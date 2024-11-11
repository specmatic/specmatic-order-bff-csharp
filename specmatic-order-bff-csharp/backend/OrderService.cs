using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using specmatic_order_bff_csharp.models;
using static specmatic_order_bff_csharp.models.Api;

namespace specmatic_order_bff_csharp.backend;

public class OrderService
{
    private readonly string _orderApiUrl = Environment.GetEnvironmentVariable("ORDER_API_URL") ?? string.Empty;
    private const string AuthToken = "API-TOKEN-SPEC";
    private HttpClient _httpClient;
    public OrderService(HttpClient httpClient = null)
    {
        _httpClient = httpClient ?? new HttpClient();
    }
    public virtual int CreateOrder(OrderRequest orderRequest)
    {
        return CreateOrderAsync(new Order(orderRequest.Productid, orderRequest.Count, "pending")).Result;
    }
    
    public virtual int CreateProduct(ProductRequest productRequest)
    {
        return CreateProductAsync(productRequest).Result;
    }

    public virtual IEnumerable<Product> FindProducts(string type)
    {
        return FindProductsAsync(type).Result;
    }

    private async Task<int> CreateOrderAsync(Order order)
    {
        var orderId = 0;
        using var client = GetHttpClient();
        var request = new HttpRequestMessage(Api.CreateOrder.Method, Api.CreateOrder.Url);
        request.Content = JsonContent.Create(order);
        
        using var response = await client.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<IdResponse>(responseString);
        if (responseObject != null) orderId = responseObject.Id;
        return orderId;
    }

    private async Task<int> CreateProductAsync(ProductRequest productRequest)
    {
        var productId = 0;
        using var client = GetHttpClient();
        var request = new HttpRequestMessage(CreateProducts.Method, CreateProducts.Url);
        request.Content = JsonContent.Create(productRequest);
        
        using var response = await client.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<IdResponse>(responseString);
        if (responseObject != null) productId = responseObject.Id;
        return productId;
    }

    private async Task<IEnumerable<Product>> FindProductsAsync(string type)
    {
        List<Product> products = [];
        using var client = GetHttpClient();
        var productEndpoint = type.Equals("")? ListProducts.Url : ListProducts.Url + $"?type={type}";
        var request = new HttpRequestMessage(HttpMethod.Get, productEndpoint);
        
        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<IEnumerable<Product>>(responseString);
        if (responseObject != null) products = responseObject.ToList();
        return products;        
    }
    
    private HttpClient GetHttpClient()
    {
        var client = _httpClient;
        client.BaseAddress = client.BaseAddress ?? new Uri(_orderApiUrl);
        client.Timeout = TimeSpan.FromSeconds(4);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        client.DefaultRequestHeaders.Add("Authenticate", AuthToken);
        return client;
    }
}