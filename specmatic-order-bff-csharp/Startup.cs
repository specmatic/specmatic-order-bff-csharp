using Microsoft.AspNetCore.Mvc;
using specmatic_order_bff_csharp.backend;
using specmatic_order_bff_csharp.services;
using specmatic_uuid_api.Models;
namespace specmatic_order_bff_csharp;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    var response = new ErrorResponse
                    {
                        TimeStamp = DateTime.UtcNow.ToString("o"),
                        Error = "Bad Request",
                        Message = string.Join(", ", errors)
                    };
                    return new BadRequestObjectResult(response);
                };
            })
            .AddXmlSerializerFormatters();

        services.AddScoped<OrderBffService>();
        services.AddScoped<OrderService>();

        services.AddHttpClient();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}