namespace specmatic_order_bff_csharp.models;

public class Api
{
    public HttpMethod Method { get; }
    public string Url { get; }
    private Api(HttpMethod method, string url)
    {
        Method = method;
        Url = url;
    }

    public static readonly Api CreateOrder = new(HttpMethod.Post, "/orders");
    public static readonly Api ListProducts = new(HttpMethod.Get, "/products");
    public static readonly Api CreateProducts = new(HttpMethod.Post, "/products");

}