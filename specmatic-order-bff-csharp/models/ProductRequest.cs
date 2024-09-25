using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace specmatic_order_bff_csharp.models;

public class ProductRequest
{
    public required string Name { get; set; }
    public required string Type { get; set; }
    [Range(1, 101)] public required int Inventory { get; set; }
}