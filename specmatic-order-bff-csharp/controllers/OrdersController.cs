using Microsoft.AspNetCore.Mvc;
using specmatic_order_bff_csharp.services;

namespace specmatic_order_bff_csharp.controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
}