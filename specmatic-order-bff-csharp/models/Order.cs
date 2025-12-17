using System.Text.Json.Serialization;
namespace specmatic_order_bff_csharp.models;

public class Order( int productid, int count, OrderStatus status)
{
    [JsonPropertyName("productid")]
    public int Productid { get; init; } = productid;

    [JsonPropertyName("count")]
    public int Count { get; init; } = count;

    [JsonPropertyName("status")]
[JsonConverter(typeof(StrictStringEnumConverter<OrderStatus>))]
    public OrderStatus Status { get; set; } = status;
}

public enum OrderStatus
{
    pending,
    fulfilled,
    cancelled
}
