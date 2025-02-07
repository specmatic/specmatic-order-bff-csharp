using System.Diagnostics;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
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

    [Fact]
    public async Task ContractTestsAsync()
    {
        await RunContractTests();
        Assert.NotNull(_testContainer);
        var logs = await _testContainer.GetLogsAsync();
        if (!logs.Stdout.Contains("Failures: 0"))
        {
            Assert.Fail("There are failing tests, please refer to build/reports/specmatic/html/index.html for more details");
        }
    }

    public async Task InitializeAsync()
    {
        await StartDomainServiceStub();
        _appProcess = StartOrderBffService();
        await TestcontainersSettings.ExposeHostPortsAsync(8080)
            .ConfigureAwait(false);
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
        if (_stubContainer != null) await _stubContainer.DisposeAsync();
    }

    private async Task RunContractTests()
    { 
        var localReportDirectory = Path.Combine(Pwd, "build", "reports");
        Directory.CreateDirectory(localReportDirectory);
        
        _testContainer = new ContainerBuilder()
            .WithImage("znsio/specmatic").WithCommand("test")
            .WithCommand("--port=8080")
            .WithCommand("--host=host.testcontainers.internal")
            .WithOutputConsumer(Consume.RedirectStdoutAndStderrToConsole())
            .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("Tests run:"))
            .WithBindMount(localReportDirectory, $"{TestContainerDirectory}/build/reports")
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
            .WithImage("znsio/specmatic").WithCommand("stub")
            .WithCommand("--examples=examples")
            .WithPortBinding(9000)
            .WithExposedPort(9000)
            .WithOutputConsumer(Consume.RedirectStdoutAndStderrToConsole())
            .WithEnvironment("EXTENSIBLE_SCHEMA", "true")
            .WithBindMount($"{Pwd}/examples/domain_service", $"{TestContainerDirectory}/examples")
            .WithBindMount($"{Pwd}/uuid.openapi.yaml", $"{TestContainerDirectory}/uuid.openapi.yaml")
            .WithBindMount($"{Pwd}/specmatic.yaml", $"{TestContainerDirectory}/specmatic.yaml")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(9000))
            .WithReuse(true)
            .Build();

        await _stubContainer.StartAsync().ConfigureAwait(false);
    }
}