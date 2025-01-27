namespace specmatic_order_bff_csharp.models;
public class Order( int productid, int count, string status)
{
    public int Productid { get; init; } = productid;
    public int Count { get; init; } = count;
    public string Status { get; init; } = status;
}