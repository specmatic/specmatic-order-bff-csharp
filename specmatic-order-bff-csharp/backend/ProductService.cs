using System.Net.Http.Headers;
using System.Text.Json;
using specmatic_order_bff_csharp.models;

namespace specmatic_order_bff_csharp.backend;

public class ProductService : IProductService
{
    private readonly string _orderApiUrl = Environment.GetEnvironmentVariable("ORDER_API_URL")!;
    private const string AuthToken = "API-TOKEN-SPEC";
    public int CreateProduct(ProductRequest productRequest)
    {
        return CreateProductAsync(productRequest).Result;
    }

    private async Task<int> CreateProductAsync(ProductRequest productRequest)
    {
        var productId = 0;
        using var client = new HttpClient();
        client.BaseAddress = new Uri(_orderApiUrl);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Authenticate", AuthToken);
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/products");
        request.Content = JsonContent.Create(productRequest);
        
        using var response = await client.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<IdResponse>(responseString);
        if (responseObject != null) productId = responseObject.id;
        return productId;
    }
}