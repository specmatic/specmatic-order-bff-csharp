using System.Text.Json.Serialization;

namespace specmatic_order_bff_csharp.models;

public class Product(int id, string name, string type, int inventory)
{
    [JsonPropertyName("id")]
    public int Id { get; } = id;
    [JsonPropertyName("name")]
    public string Name { get; } = name;
    [JsonPropertyName("type")]
    public string Type { get; } = type;
    [JsonPropertyName("inventory")]
    public int Inventory { get; } = inventory;
}