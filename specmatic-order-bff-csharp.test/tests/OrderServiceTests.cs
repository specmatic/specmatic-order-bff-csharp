using Moq;
using Xunit;
using specmatic_order_bff_csharp.models;
using specmatic_order_bff_csharp.backend;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Moq.Protected;
namespace specmatic_order_bff_csharp.test.tests;
public class OrderServiceTests
{
    private readonly OrderService _orderService;
    private readonly Mock<HttpMessageHandler> _mockHandler;
    public OrderServiceTests()
    {
        // Mock HttpMessageHandler to simulate HttpClient behavior
        _mockHandler = new Mock<HttpMessageHandler>(); 
        // Create HttpClient using the mocked handler
        var httpClient = new HttpClient(_mockHandler.Object)
        {
            BaseAddress = new Uri("https://dummyapi.com/") // Set the base address as needed
        };

         _orderService = new OrderService(httpClient); // Pass HttpClient to OrderService
    }

    [Fact]
    public void CreateOrder_ShouldReturnOrderId_WhenOrderIsCreated()
    {
        // Arrange
        var orderRequest = new OrderRequest(count:2,productId:2);
        var expectedOrderId = 123;

        // Set up the mock response for the HTTP request
        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new IdResponse(id: expectedOrderId)))
        };

        // Mock SendAsync to return the mockResponse for CreateOrder
        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockResponse);

        // Act
        var result =  _orderService.CreateOrder(orderRequest);

        // Assert
        Assert.Equal(expectedOrderId, result);
    }

    [Fact]
    public  void CreateProduct_ShouldReturnProductId_WhenProductIsCreated()
    {
        // Arrange
        var productRequest = new ProductRequest("iPhone", "goods", 100);
        var expectedProductId = 456;

        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new IdResponse ( id: expectedProductId )))
        };

        // Mock SendAsync to return the mockResponse for CreateProduct
        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockResponse);

        // Act
        var result =  _orderService.CreateProduct(productRequest);

        // Assert
        Assert.Equal(expectedProductId, result);
    }

    [Fact]
    public  void FindProducts_ShouldReturnListOfProducts_WhenProductsAreFound()
    {
        // Arrange
        var productType = "Electronics";
        var expectedProducts = new List<Product>
        {
            new Product(id: 1, name: "Phone", type: "Electronics", inventory: 100),
            new Product(id: 2, name: "Laptop", type: "Electronics", inventory: 100)
        };

        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(expectedProducts))
        };

        // Mock SendAsync to return the mockResponse for FindProducts
        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockResponse);

        // Act
        var result =  _orderService.FindProducts(productType);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProducts.Count, result.Count());
        Assert.Collection(result,
            item => Assert.Equal(expectedProducts[0].Id, item.Id),
            item => Assert.Equal(expectedProducts[1].Id, item.Id)
            // Add more checks for each item based on properties
        );

    }
}
