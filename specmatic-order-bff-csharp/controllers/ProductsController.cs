using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using specmatic_order_bff_csharp.models;
using specmatic_order_bff_csharp.services;
using ValidationException = specmatic_order_bff_csharp.exceptions.ValidationException;
using System.Diagnostics.CodeAnalysis;
namespace specmatic_order_bff_csharp.controllers;

[ApiController]
[Route("[controller]")]
[ExcludeFromCodeCoverage]
public class ProductsController(OrderBffService orderBffService) : ControllerBase
{
    [HttpPost]
    public IActionResult CreateProduct(ProductRequest productRequest)
    {
        return StatusCode(StatusCodes.Status201Created, orderBffService.CreateProduct(productRequest));
    }

    [HttpGet("/findAvailableProducts")]
    public IActionResult FindAvailableProducts([FromHeader, Required]int pageSize, [FromQuery]string? type = "")
    {
        if (pageSize<0) return BadRequest(new {Error ="Page size cannot be less than zero"});
        try
        {
            return StatusCode(StatusCodes.Status200OK, orderBffService.FindProducts(type ?? string.Empty));
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new ValidationException("Request Timed out"));
        }
    }
}