using InnoShop.ProductApplication.Contracts.Persistence;
using InnoShop.ProductApplication.Contracts.Services;
using InnoShop.ProductApplication.Features.Products.Commands.DeleteProduct;
using InnoShop.ProductDomain.Entities;
using InnoShop.Shared.Exceptions;
using Moq;
using Xunit;

namespace InnoShop.ProductApplication.UnitTests.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly DeleteProductCommandHandler _handler;

        public DeleteProductCommandHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _handler = new DeleteProductCommandHandler(_productRepositoryMock.Object, _currentUserServiceMock.Object);
        }

        [Fact]
        public async Task Handle_Should_DeleteProduct_When_UserIsOwner()
        {
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                UserId = userId,
                Name = "N",
                Description = "D",
                Price = 10,
                IsAvailable = true
            };

            _currentUserServiceMock.Setup(s => s.UserGuid).Returns(userId);
            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);

            var command = new DeleteProductCommand { ProductId = productId };

            await _handler.Handle(command, CancellationToken.None);

            _productRepositoryMock.Verify(r => r.DeleteAsync(product), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowForbidden_When_UserIsNotOwner()
        {
            var ownerId = Guid.NewGuid();
            var otherId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                UserId = ownerId,
                Name = "N",
                Description = "D",
                Price = 10,
                IsAvailable = true
            };

            _currentUserServiceMock.Setup(s => s.UserGuid).Returns(otherId);
            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);

            var command = new DeleteProductCommand { ProductId = productId };

            await Assert.ThrowsAsync<ForbiddenException>(() => _handler.Handle(command, CancellationToken.None));
            _productRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Product>()), Times.Never);
        }
    }
}