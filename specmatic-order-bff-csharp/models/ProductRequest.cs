using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace specmatic_order_bff_csharp.models;

[method: SetsRequiredMembers]
public class ProductRequest(string name, string type, int inventory)
{
    public required string Name { get; init; } = name;
    public required string Type { get; init; } = type;
    [Range(1, 101)] public required int Inventory { get; init; } = inventory;
}