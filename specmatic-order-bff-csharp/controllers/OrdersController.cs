using Microsoft.AspNetCore.Mvc;
using specmatic_order_bff_csharp.models;
using specmatic_order_bff_csharp.services;

namespace specmatic_order_bff_csharp.controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController(OrderBffService orderBffService) : ControllerBase
{
    [HttpPost]
    public IActionResult CreateOrder([FromBody]OrderRequest orderRequest)
    {
        return StatusCode(StatusCodes.Status201Created, orderBffService.CreateOrder(orderRequest));
    }
}