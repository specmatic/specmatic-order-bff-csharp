namespace specmatic_order_bff_csharp.models;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class OrderRequest(int productId, int count)
{
    public int Count { get; } = count;
    public int ProductId { get; } = productId;
}