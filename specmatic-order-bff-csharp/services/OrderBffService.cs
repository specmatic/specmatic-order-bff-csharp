using specmatic_order_bff_csharp.backend;
using specmatic_order_bff_csharp.models;

namespace specmatic_order_bff_csharp.services;

public class OrderBffService(IOrderService orderService) : IOrderBffService
{
    public IdResponse CreateOrder(OrderRequest orderRequest)
    {
        var id = orderService.CreateOrder(orderRequest);
        return new IdResponse(id);
    }
}