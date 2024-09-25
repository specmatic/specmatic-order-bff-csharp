using Microsoft.AspNetCore.Mvc;
using specmatic_order_bff_csharp.models;
using specmatic_order_bff_csharp.services;

namespace specmatic_order_bff_csharp.controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController(IProductBffService productBffService) : ControllerBase
{
    [HttpPost]
    public IActionResult CreateProduct(ProductRequest productRequest)
    {
        return StatusCode(StatusCodes.Status201Created, productBffService.CreateProduct(productRequest));
    }
}