using specmatic_order_bff_csharp.models;

namespace specmatic_order_bff_csharp.services;

public interface IProductBffService
{
    IdResponse CreateProduct(ProductRequest productRequest);
    
    IEnumerable<Product> FindProducts(string type);
}