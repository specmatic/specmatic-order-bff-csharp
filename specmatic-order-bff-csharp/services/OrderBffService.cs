using specmatic_order_bff_csharp.backend;
using specmatic_order_bff_csharp.models;

namespace specmatic_order_bff_csharp.services;

public class OrderBffService(OrderService orderService)
{
    public IdResponse CreateOrder(OrderRequest orderRequest)
    {
        var id = orderService.CreateOrder(orderRequest);
        return new IdResponse(id);
    }
    public IdResponse CreateProduct(ProductRequest productRequest)
    {
        var id = orderService.CreateProduct(productRequest);
        return new IdResponse(id);
    }

    public IEnumerable<Product> FindProducts(string type)
    {
        return orderService.FindProducts(type);
    }
}