namespace specmatic_order_bff_csharp.models;

public class Order(int id, string status, int count, int productid)
{
    public int Count { get; set; } = count;

    public string Status { get; init; } = status;

    public int Productid { get; init; } = productid;
}