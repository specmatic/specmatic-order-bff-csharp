namespace specmatic_order_bff_csharp.models;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class Order( int productId, int count, string status)
{
    public int ProductId { get; init; } = productId;
    public int Count { get; init; } = count;
    public string Status { get; init; } = status;
}