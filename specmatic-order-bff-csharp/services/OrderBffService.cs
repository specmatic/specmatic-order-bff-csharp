using specmatic_order_bff_csharp.backend;
using specmatic_order_bff_csharp.models;

namespace specmatic_order_bff_csharp.services;

public class OrderBffService(IOrderService orderService) : IOrderBFFService
{
    public OrderResponse CreateOrder(OrderRequest orderRequest)
    {
        var id = orderService.CreateOrder(orderRequest);
        return new OrderResponse(id);
    }
}