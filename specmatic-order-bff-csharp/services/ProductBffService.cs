using specmatic_order_bff_csharp.backend;
using specmatic_order_bff_csharp.models;

namespace specmatic_order_bff_csharp.services;

public class ProductBffService(IProductService productService) : IProductBffService
{
    public IdResponse CreateProduct(ProductRequest productRequest)
    {
        var id = productService.CreateProduct(productRequest);
        return new IdResponse(id);
    }

    public IEnumerable<Product> FindProducts(string type)
    {
        return productService.FindProducts(type);
    }
}