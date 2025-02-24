using Moq;
using ProductPriceNegotiationApi.Models;
using ProductPriceNegotiationApi.Repositories;
using ProductPriceNegotiationApi.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _productRepoMock = new Mock<IProductRepository>();
        _productService = new ProductService(_productRepoMock.Object);
    }

    [Fact]
    public async Task AddProduct_ShouldAddProduct_WhenValid()
    {
        // Arrange
        var product = new Product { Name = "Laptop", Price = 5000.00m };

        // Act
        await _productService.AddProduct(product);

        // Assert
        _productRepoMock.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task AddProduct_ShouldThrowException_WhenNameIsEmpty()
    {
        // Arrange
        var product = new Product { Name = "", Price = 5000.00m };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _productService.AddProduct(product));
    }

    [Fact]
    public async Task AddProduct_ShouldThrowException_WhenPriceIsZero()
    {
        // Arrange
        var product = new Product { Name = "Laptop", Price = 0m };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _productService.AddProduct(product));
    }

    [Fact]
    public async Task GetProduct_ShouldReturnProduct_WhenExists()
    {
        // Arrange
        var productId = 1;
        var product = new Product { Id = productId, Name = "Laptop", Price = 5000.00m };

        _productRepoMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);

        // Act
        var result = await _productService.GetProduct(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
    }

    [Fact]
    public async Task GetProduct_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        _productRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product?)null);

        // Act
        var result = await _productService.GetProduct(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturnProductList()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 5000.00m },
            new Product { Id = 2, Name = "Smartphone", Price = 3000.00m }
        };

        _productRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _productService.GetAllProducts();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.ToList().Count);
    }
}
