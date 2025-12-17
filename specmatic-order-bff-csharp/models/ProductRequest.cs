using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace specmatic_order_bff_csharp.models;

[method: SetsRequiredMembers]
public class ProductRequest(string name, ProductType type, int inventory)
{
    [JsonPropertyName("name")]
    public required string Name { get; init; } = name;

    [JsonPropertyName("type")]
    [JsonConverter(typeof(StrictStringEnumConverter<ProductType>))]
    public required ProductType Type { get; init; } = type;

    [JsonPropertyName("inventory")]
    [Range(1, 101)] public required int Inventory { get; init; } = inventory;
}