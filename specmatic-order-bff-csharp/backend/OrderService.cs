using System.Net.Http.Headers;
using System.Text.Json;
using specmatic_order_bff_csharp.models;

namespace specmatic_order_bff_csharp.backend;

public class OrderService : IOrderService
{
    private readonly string _orderApiUrl = Environment.GetEnvironmentVariable("ORDER_API_URL")!;
    private const string AuthToken = "API-TOKEN-SPEC";

    public int CreateOrder(OrderRequest orderRequest)
    {
        return CreateOrderAsync(new Order(orderRequest.Productid, orderRequest.Count, "pending")).Result;
    }

    private async Task<int> CreateOrderAsync(Order order)
    {
        var orderId = 0;
        using var client = new HttpClient();
        client.BaseAddress = new Uri(_orderApiUrl);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Authenticate", AuthToken);
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/orders");
        request.Content = JsonContent.Create(order);
        
        using var response = await client.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<OrderResponse>(responseString);
        if (responseObject != null) orderId = responseObject.Id;
        return orderId;
    }
}