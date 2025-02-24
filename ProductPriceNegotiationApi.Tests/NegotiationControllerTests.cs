using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using ProductPriceNegotiationApi;
using ProductPriceNegotiationApi.Models;
using Xunit;

public class NegotiationControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public NegotiationControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<int> EnsureProductExists()
    {
        // Arrange
        var createProductRequest = new { Name = "Laptop", Price = 5000.00m };

        // Act
        var productResponse = await _client.PostAsJsonAsync("/api/products", createProductRequest);

        // Assert
        if (productResponse.StatusCode == HttpStatusCode.Created)
        {
            var product = await productResponse.Content.ReadFromJsonAsync<Product>();
            return product?.Id ?? 0;
        }

        return 0;
    }

    [Fact]
    public async Task StartNegotiation_ShouldReturn201_WhenValid()
    {
        // Arrange
        int productId = await EnsureProductExists();
        Assert.True(productId > 0, "Product was not created successfully.");

        var request = new { proposedPrice = 4000.00m };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/negotiations/{productId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task StartNegotiation_ShouldReturn400_WhenInvalidPrice()
    {
        // Arrange
        int productId = await EnsureProductExists();
        Assert.True(productId > 0, "Product was not created successfully.");

        var request = new { proposedPrice = 0 };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/negotiations/{productId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetNegotiation_ShouldReturn404_WhenNotFound()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/api/negotiations/999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
