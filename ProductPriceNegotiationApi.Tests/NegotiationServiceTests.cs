using Moq;
using ProductPriceNegotiationApi.Models;
using ProductPriceNegotiationApi.Repositories;
using ProductPriceNegotiationApi.Services;
using System;
using System.Threading.Tasks;
using Xunit;

public class NegotiationServiceTests
{
    private readonly Mock<INegotiationRepository> _negotiationRepoMock;
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly NegotiationService _negotiationService;

    public NegotiationServiceTests()
    {
        _negotiationRepoMock = new Mock<INegotiationRepository>();
        _productRepoMock = new Mock<IProductRepository>();
        _negotiationService = new NegotiationService(_negotiationRepoMock.Object, _productRepoMock.Object);
    }

   [Fact]
public async Task StartNegotiation_ShouldCreateNegotiation_WhenValid()
{
    // Arrange
    var productId = 1;
    var price = 4000m;
    var fakeNegotiation = new Negotiation { Id = 1, ProductId = productId, ProposedPrice = price };

    _productRepoMock.Setup(repo => repo.GetByIdAsync(productId))
        .ReturnsAsync(new Product { Id = productId, Name = "Laptop", Price = 4500m });

    _negotiationRepoMock.Setup(repo => repo.AddAsync(It.IsAny<Negotiation>()))
        .Callback<Negotiation>(n => n.Id = 1)
        .Returns(Task.CompletedTask);

    // Act
    var negotiationId = await _negotiationService.StartNegotiation(productId, price);

    // Assert
    Assert.True(negotiationId > 0);
    _negotiationRepoMock.Verify(repo => repo.AddAsync(It.IsAny<Negotiation>()), Times.Once);
}


    [Fact]
    public async Task StartNegotiation_ShouldThrowException_WhenProductNotFound()
    {
        // Arrange
        _productRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _negotiationService.StartNegotiation(1, 4000m));
    }

    [Fact]
    public async Task RespondToNegotiation_ShouldAcceptNegotiation()
    {
        // Arrange
        var negotiationId = 1;
        var negotiation = new Negotiation { Id = negotiationId, Attempts = 1 };
        _negotiationRepoMock.Setup(repo => repo.GetByIdAsync(negotiationId)).ReturnsAsync(negotiation);

        // Act
        await _negotiationService.RespondToNegotiation(negotiationId, true);

        // Assert
        Assert.True(negotiation.IsAccepted);
        _negotiationRepoMock.Verify(repo => repo.UpdateAsync(negotiation), Times.Once);
    }

    [Fact]
    public async Task RespondToNegotiation_ShouldThrowException_WhenNegotiationNotFound()
    {
        // Arrange
        _negotiationRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Negotiation?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _negotiationService.RespondToNegotiation(1, true));
    }
}
