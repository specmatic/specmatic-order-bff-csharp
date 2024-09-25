namespace specmatic_order_bff_csharp.models;

public class OrderRequest( int productid, int count)
{
    public int Count { get; set; } = count;
    public int Productid { get; init; } = productid;
}