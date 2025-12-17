using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using specmatic_order_bff_csharp.models;
using static specmatic_order_bff_csharp.models.Api;

namespace specmatic_order_bff_csharp.backend;

public class OrderService(HttpClient httpClient)
{
    private readonly string _orderApiUrl = Environment.GetEnvironmentVariable("ORDER_API_URL") ?? string.Empty;
    private const string AuthToken = "API-TOKEN-SPEC";

    public virtual int CreateOrder(OrderRequest orderRequest)
    {
        return CreateOrderAsync(new Order(orderRequest.Productid, orderRequest.Count, OrderStatus.pending)).Result;
    }
    
    public virtual int CreateProduct(ProductRequest productRequest)
    {
        return CreateProductAsync(productRequest).Result;
    }

    public virtual IEnumerable<Product> FindProducts(ProductType? type)
    {
        return FindProductsAsync(type).Result;
    }

    private async Task<int> HandleIdResponse(HttpResponseMessage response)
    {
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<IdResponse>(responseString);
        return responseObject?.Id ?? 0;
    }

    private async Task<int> CreateOrderAsync(Order order)
    {
        using var client = ConfiguredHttpClient();
        var request = new HttpRequestMessage(Api.CreateOrder.Method, Api.CreateOrder.Url);
        request.Content = JsonContent.Create(order);
        
        using var response = await client.SendAsync(request);
        return await HandleIdResponse(response);
    }

    private async Task<int> CreateProductAsync(ProductRequest productRequest)
    {
        using var client = ConfiguredHttpClient();
        var request = new HttpRequestMessage(CreateProducts.Method, CreateProducts.Url);
        request.Content = JsonContent.Create(productRequest);
        
        using var response = await client.SendAsync(request);
        return await HandleIdResponse(response);
    }

    private async Task<IEnumerable<Product>> FindProductsAsync(ProductType? type)
    {
        List<Product> products = [];
        using var client = ConfiguredHttpClient();
        var productEndpoint = type is null ? ListProducts.Url : ListProducts.Url + $"?type={type}";
        var request = new HttpRequestMessage(HttpMethod.Get, productEndpoint);
        
        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<IEnumerable<Product>>(responseString);
        if (responseObject != null) products = responseObject.ToList();
        return products;        
    }
    
    private HttpClient ConfiguredHttpClient()
    {
        httpClient.BaseAddress ??= new Uri(_orderApiUrl);
        httpClient.Timeout = TimeSpan.FromSeconds(4);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        httpClient.DefaultRequestHeaders.Add("Authenticate", AuthToken);
        return httpClient;
    }
}