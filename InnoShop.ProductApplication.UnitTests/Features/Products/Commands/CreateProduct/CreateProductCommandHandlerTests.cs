using FluentAssertions;
using InnoShop.ProductApplication.Contracts.Persistence;
using InnoShop.ProductApplication.Contracts.Services;
using InnoShop.ProductApplication.Features.Products.Commands.CreateProduct;
using InnoShop.ProductDomain.Entities;
using Moq;
using Xunit;

namespace InnoShop.ProductApplication.UnitTests.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;

        public CreateProductCommandHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
        }

        [Fact]
        public async Task Handle_Should_CreateProduct_When_UserIsAuthorized()
        {
            var userId = Guid.NewGuid();

            _currentUserServiceMock.Setup(x => x.UserGuid).Returns(userId);

            var command = new CreateProductCommand
            {
                Name = " New Phone ", 
                Description = "Description",
                Price = 1000
            };

            var handler = new CreateProductCommandHandler(_productRepositoryMock.Object, _currentUserServiceMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeEmpty();

            _productRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Product>(p =>
                p.Name == "New Phone" && 
                p.Price == 1000 &&
                p.UserId == userId &&
                p.IsAvailable == true
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_UserIsNotAuthorized()
        {
            _currentUserServiceMock.Setup(x => x.UserGuid).Returns((Guid?)null);

            var command = new CreateProductCommand
            {
                Name = "Phone",
                Price = 100,
                Description = "Test Description" 
            };

            var handler = new CreateProductCommandHandler(_productRepositoryMock.Object, _currentUserServiceMock.Object);

            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("*аутентифицирован*"); 
        }
    }
}