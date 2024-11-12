using Microsoft.AspNetCore.Mvc;
using specmatic_order_bff_csharp.models;
using specmatic_order_bff_csharp.services;
using System.Diagnostics.CodeAnalysis;
namespace specmatic_order_bff_csharp.controllers;

[ApiController]
[Route("[controller]")]
[ExcludeFromCodeCoverage]
public class OrdersController(OrderBffService orderBffService) : ControllerBase
{
    [HttpPost]
    public IActionResult CreateOrder([FromBody]OrderRequest orderRequest)
    {
        return StatusCode(StatusCodes.Status201Created, orderBffService.CreateOrder(orderRequest));
    }
}