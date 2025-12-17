using Moq;
using specmatic_order_bff_csharp.models;
using specmatic_order_bff_csharp.backend;
using specmatic_order_bff_csharp.services;
namespace specmatic_order_bff_csharp.test.tests;
public class OrderBffServiceTests
{
    private readonly OrderBffService _orderBffService;
    private readonly Mock<OrderService> _mockOrderService;
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    public OrderBffServiceTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var mockHttpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _mockOrderService = new Mock<OrderService>(mockHttpClient) { CallBase = true };
        _orderBffService = new OrderBffService(_mockOrderService.Object);
      
    }

    [Fact]
    public void CreateOrder_ShouldReturnIdResponse_WhenOrderIsCreated()
    {
        // Arrange
      
        var orderRequest = new OrderRequest(count:2,productid:2);
        var expectedId = 123;
        _mockOrderService.Setup(service => service.CreateOrder(It.IsAny<OrderRequest>())).Returns(expectedId);

        // Act
        var result = _orderBffService.CreateOrder(orderRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedId, result.Id);
    }

    [Fact]
    public void CreateProduct_ShouldReturnIdResponse_WhenProductIsCreated()
    {
        // Arrange
        var productRequest = new ProductRequest("iPhone", ProductType.gadget, 100);
        var expectedId = 456;
        _mockOrderService.Setup(service => service.CreateProduct(It.IsAny<ProductRequest>())).Returns(expectedId);

        // Act
        var result = _orderBffService.CreateProduct(productRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedId, result.Id);
    }

    [Fact]
    public void FindProducts_ShouldReturnListOfProducts_WhenProductsAreFound()
    {
        // Arrange
        var productType = ProductType.gadget;
        var expectedProducts = new List<Product>
        {
            new Product(id: 1,name: "Phone", productType: ProductType.gadget, inventory: 100),
            new Product(id: 2, name: "Laptop", productType: ProductType.gadget, inventory: 100)
        };
        _mockOrderService.Setup(service => service.FindProducts(productType)).Returns(expectedProducts);

        // Act
        var result = _orderBffService.FindProducts(productType);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProducts.Count, result.Count());
        Assert.Equal(expectedProducts, result);
    }
}
