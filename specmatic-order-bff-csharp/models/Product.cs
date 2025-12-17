using System.Text.Json.Serialization;

namespace specmatic_order_bff_csharp.models;
public class Product(int id, string name, ProductType productType, int inventory)
{
    [JsonPropertyName("id")]
    public int Id { get; } = id;

    [JsonPropertyName("name")]
    public string Name { get; } = name;

    [JsonPropertyName("type")]
    [JsonConverter(typeof(StrictStringEnumConverter<ProductType>))]
    public ProductType ProductType { get; } = productType;

    [JsonPropertyName("inventory")]
    public int Inventory { get; } = inventory;
}

public enum ProductType
{
    book,
    food,
    gadget,
    other
}
