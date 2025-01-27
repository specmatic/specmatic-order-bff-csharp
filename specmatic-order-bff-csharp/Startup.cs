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
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                    var error = string.Join(", ", errors);
                    return new BadRequestObjectResult(new ValidationException(error));
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