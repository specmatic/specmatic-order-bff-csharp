using specmatic_order_bff_csharp.models;

namespace specmatic_order_bff_csharp.backend;

public interface IOrderService
{
    int CreateOrder(OrderRequest orderRequest);
}