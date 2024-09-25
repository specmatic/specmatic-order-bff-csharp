namespace specmatic_order_bff_csharp.models;

public class Product(int id, string name, string type, int inventory)
{
    public int id { get; set; } = id;
    public string name { get; set; } = name;
    public string type { get; set; } = type;
    public int inventory { get; set; } = inventory;
}