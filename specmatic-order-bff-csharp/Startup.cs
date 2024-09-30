using Microsoft.AspNetCore.Mvc;
using specmatic_order_bff_csharp.backend;
using specmatic_order_bff_csharp.services;
using ValidationException = specmatic_order_bff_csharp.exceptions.ValidationException;

namespace specmatic_order_bff_csharp;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = _ =>
                    new ObjectResult(new ValidationException("Bad request"))
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
            })
            .AddXmlSerializerFormatters();

        services.AddSingleton<OrderBffService>();
        services.AddSingleton<OrderService>();
        
        services.AddHttpClient();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}