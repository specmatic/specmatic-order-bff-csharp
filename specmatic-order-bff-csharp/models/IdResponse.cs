using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;
namespace specmatic_order_bff_csharp.models;

[ExcludeFromCodeCoverage]
public class IdResponse(int id)
{
    [JsonPropertyName("id")]public int Id { get; init; } = id;
}