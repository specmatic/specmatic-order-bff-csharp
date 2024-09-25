using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace specmatic_order_bff_csharp.models;

public class ProductRequest
{
    public string name { get; set; }
    public string type { get; set; }
    [Range(1, 101)] public int inventory { get; set; }
}