namespace specmatic_order_bff_csharp.exceptions;
public class ValidationException(string error)
{
    public string Error { get; init; } = error;
}