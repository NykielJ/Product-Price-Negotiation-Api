using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductPriceNegotiationApi.Controllers;
using ProductPriceNegotiationApi.Models;
using ProductPriceNegotiationApi.Repositories;
using ProductPriceNegotiationApi.Services;
using Xunit;

public class ProductControllerTests
{
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly ProductService _productService;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {

        _productRepoMock = new Mock<IProductRepository>();
        _productService = new ProductService(_productRepoMock.Object);
        _controller = new ProductController(_productService);
    }

    [Fact]
    public async Task AddProduct_ShouldReturn201_WhenValid()
    {
        // Arrange
        var product = new Product { Name = "Laptop", Price = 5000.00m };

        // Act
        var result = await _controller.AddProduct(product) as CreatedAtActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(201, result.StatusCode);
        _productRepoMock.Verify(s => s.AddAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task AddProduct_ShouldReturn400_WhenInvalid()
    {
        // Arrange
        var product = new Product { Name = "", Price = 0m };
        _controller.ModelState.AddModelError("Name", "Product name is required");

        // Act
        var result = await _controller.AddProduct(product) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public async Task GetProduct_ShouldReturn200_WhenExists()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Laptop", Price = 5000.00m };
        _productRepoMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _controller.GetProduct(1) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(product, result.Value);
    }

    [Fact]
    public async Task GetProduct_ShouldReturn404_WhenNotExists()
    {
        // Arrange
        _productRepoMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Product?)null);

        // Act
        var result = await _controller.GetProduct(1) as NotFoundObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturn200_WithListOfProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 5000.00m },
            new Product { Id = 2, Name = "Smartphone", Price = 3000.00m }
        };
        _productRepoMock.Setup(s => s.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _controller.GetAllProducts() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        var returnedProducts = Assert.IsType<List<Product>>(result.Value);
        Assert.Equal(2, returnedProducts.Count);
    }
}
