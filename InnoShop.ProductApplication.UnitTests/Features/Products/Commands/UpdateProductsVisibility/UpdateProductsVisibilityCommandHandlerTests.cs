using InnoShop.ProductApplication.Contracts.Persistence;
using InnoShop.ProductApplication.Features.Products.Commands.UpdateProductsVisibility;
using Moq;
using Xunit;

namespace InnoShop.ProductApplication.UnitTests.Features.Products.Commands.UpdateProductsVisibility
{
    public class UpdateProductsVisibilityCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly UpdateProductsVisibilityCommandHandler _handler;

        public UpdateProductsVisibilityCommandHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _handler = new UpdateProductsVisibilityCommandHandler(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_CallRepositoryMethod()
        {
            var command = new UpdateProductsVisibilityCommand
            {
                UserId = Guid.NewGuid(),
                IsVisible = false
            };

            await _handler.Handle(command, CancellationToken.None);

            _productRepositoryMock.Verify(r => r.SetAvailabilityForUserProductsAsync(command.UserId, command.IsVisible), Times.Once);
        }
    }
}