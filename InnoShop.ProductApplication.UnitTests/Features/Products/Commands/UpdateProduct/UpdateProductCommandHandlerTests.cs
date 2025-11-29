using FluentAssertions;
using InnoShop.ProductApplication.Contracts.Persistence;
using InnoShop.ProductApplication.Contracts.Services;
using InnoShop.ProductApplication.Features.Products.Commands.UpdateProduct;
using InnoShop.ProductDomain.Entities;
using InnoShop.Shared.Exceptions; 
using Moq;
using Xunit;
using MediatR;

namespace InnoShop.ProductApplication.UnitTests.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly UpdateProductCommandHandler _handler;

        public UpdateProductCommandHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _handler = new UpdateProductCommandHandler(_productRepositoryMock.Object, _currentUserServiceMock.Object);
        }

        [Fact]
        public async Task Handle_Should_UpdateProduct_When_UserIsOwner()
        {
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var existingProduct = new Product
            {
                Id = productId,
                UserId = userId, 
                Name = "Old Name",
                Description = "Old Desc",
                Price = 10,
                IsAvailable = true
            };

            _currentUserServiceMock.Setup(x => x.UserGuid).Returns(userId);
            _productRepositoryMock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(existingProduct);

            var command = new UpdateProductCommand
            {
                ProductId = productId,
                Name = "New Name",
                Description = "New Desc",
                Price = 20,
                IsAvailable = false
            };

            await _handler.Handle(command, CancellationToken.None);

            _productRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Product>(p =>
                p.Name == "New Name" &&
                p.Price == 20 &&
                p.IsAvailable == false
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowForbidden_When_UserIsNotOwner()
        {
            var ownerId = Guid.NewGuid();
            var hackerId = Guid.NewGuid(); 
            var productId = Guid.NewGuid();

            var existingProduct = new Product
            {
                Id = productId,
                UserId = ownerId, 
                Name = "Name",
                Description = "Desc",
                Price = 10,
                IsAvailable = true
            };

            _currentUserServiceMock.Setup(x => x.UserGuid).Returns(hackerId); 
            _productRepositoryMock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(existingProduct);

            var command = new UpdateProductCommand
            {
                ProductId = productId,
                Name = "Hacked",
                Description = "D",
                Price = 1,
                IsAvailable = true
            };

            await Assert.ThrowsAsync<ForbiddenException>(() => _handler.Handle(command, CancellationToken.None));

            _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFound_When_ProductDoesNotExist()
        {
            _currentUserServiceMock.Setup(x => x.UserGuid).Returns(Guid.NewGuid());
            _productRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product?)null);

            var command = new UpdateProductCommand
            {
                ProductId = Guid.NewGuid(),
                Name = "N",
                Description = "D",
                Price = 1,
                IsAvailable = true
            };

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}