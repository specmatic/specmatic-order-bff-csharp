using System.Diagnostics;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace specmatic_order_bff_csharp.test.contract;

public class ContractTests : IAsyncLifetime
{
    private IContainer? _stubContainer, _testContainer;
    private Process? _appProcess;
    private static readonly string Pwd =
        Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName ?? string.Empty;
    private readonly string _projectPath = Directory.GetParent(Pwd)?.FullName ?? string.Empty;
    private const string ProjectName = "specmatic-order-bff-csharp/specmatic-order-bff-csharp.csproj";
    private const string TestContainerDirectory = "/usr/src/app";

     private readonly string localReportDirectory = Path.Combine(Pwd, "build", "reports");

    [Fact]
    public async Task ContractTestsAsync()
    {
        await RunContractTests();
        Assert.NotNull(_testContainer);
        var exit = await _testContainer.GetExitCodeAsync();
        var logs = await _testContainer.GetLogsAsync();
        if (exit != 0 || !logs.Stdout.Contains("Failures: 0"))
        {
            throw new Exception("Contract tests failed with exit code: " + exit);
        }
    }

    public async Task InitializeAsync()
    {
        Directory.CreateDirectory(localReportDirectory);
        await StartDomainServiceStub();
        _appProcess = StartOrderBffService();
    }

    public async Task DisposeAsync()
    {
        if (_appProcess != null && !_appProcess.HasExited)
        {
            _appProcess.Kill();
            await _appProcess.WaitForExitAsync();
            _appProcess.Dispose();
        }
        if (_testContainer != null) await _testContainer.DisposeAsync();
        if (_stubContainer != null) {
            await _stubContainer.StopAsync(); 
            await _stubContainer.DisposeAsync();
        }
    }

    private async Task RunContractTests()
    { 
        _testContainer = new ContainerBuilder()
            .WithImage("specmatic/specmatic").WithCommand("test")
            .WithCommand("--port=8080")
            .WithCommand("--host=host.docker.internal")
            .WithCommand("--filter=PATH!='/health'") 
            .WithOutputConsumer(Consume.RedirectStdoutAndStderrToConsole())
            .WithBindMount(localReportDirectory, $"{TestContainerDirectory}/build/reports")
            .WithExtraHost("host.docker.internal", "host-gateway")
            .WithBindMount(
                $"{Pwd}/specmatic.yaml",
                $"{TestContainerDirectory}/specmatic.yaml").Build();
         
        await _testContainer.StartAsync().ConfigureAwait(true);
    }

    private Process StartOrderBffService()
    {
        var appProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --project {_projectPath}/{ProjectName}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        appProcess.Start();
        Console.WriteLine($"Order BFF service started on id: {appProcess.Id}");
        return appProcess;
    }

    private async Task StartDomainServiceStub()
    {
        _stubContainer = new ContainerBuilder()
            .WithImage("specmatic/specmatic").WithCommand("stub")
            .WithCommand("--examples=examples")
            .WithPortBinding(9000)
            .WithOutputConsumer(Consume.RedirectStdoutAndStderrToConsole())
            .WithExposedPort(9000)
            .WithBindMount($"{Pwd}/examples/domain_service", $"{TestContainerDirectory}/examples")
            .WithBindMount(localReportDirectory, $"{TestContainerDirectory}/build/reports")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilInternalTcpPortIsAvailable(9000))
            .WithBindMount(
                $"{Pwd}/specmatic.yaml",
                $"{TestContainerDirectory}/specmatic.yaml").Build();
        
        await _stubContainer.StartAsync().ConfigureAwait(false);
    }
}