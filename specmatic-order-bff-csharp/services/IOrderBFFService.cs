using specmatic_order_bff_csharp.models;

namespace specmatic_order_bff_csharp.services;

public interface IOrderBffService
{
    IdResponse CreateOrder(OrderRequest orderRequest);
}