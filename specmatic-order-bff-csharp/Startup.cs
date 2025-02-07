using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using specmatic_order_bff_csharp.backend;
using specmatic_order_bff_csharp.models;
using specmatic_order_bff_csharp.services;
using ValidationException = specmatic_order_bff_csharp.exceptions.ValidationException;
namespace specmatic_order_bff_csharp;
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                var error = string.Join(", ", errors);
                return new BadRequestObjectResult(new ValidationException(error));
            };
        })
        .AddXmlSerializerFormatters()
        .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddScoped<OrderBffService>();
        services.AddScoped<OrderService>();

        services.AddHttpClient();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment environment, IHttpClientFactory httpClientFactory)
    {
        var httpClient = httpClientFactory.CreateClient();
        var uuid = GetUuidAsync(httpClient);
        if (uuid != null) Environment.SetEnvironmentVariable("UUID", uuid);

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }

    private static string? GetUuidAsync(HttpClient httpClient)
    {
        try
        {
            var request = new HttpRequestMessage(Api.GetUuid.Method, Api.GetUuid.Url)
            {
                Content = JsonContent.Create(new UuidRequest())
            };
            httpClient.BaseAddress ??= new Uri(Environment.GetEnvironmentVariable("ORDER_API_URL") ?? string.Empty);
            using var response = httpClient.Send(request);
            response.EnsureSuccessStatusCode();
            var responseString = response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<UuidResponse>(responseString.Result);
            return responseObject?.Uuid.ToString() ?? null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching UUID during startup: {ex}");
            return null;
        }
    }
}