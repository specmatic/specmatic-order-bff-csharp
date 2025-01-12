namespace specmatic_order_bff_csharp.exceptions;
using System.Diagnostics.CodeAnalysis;
public class ValidationException(string error)
{
    public string Error { get; init; } = error;
}