using InnoShop.Application.Contracts.Infrastructure;
using InnoShop.Application.Contracts.Persistence;
using InnoShop.Application.Features.Users.Commands.UpdateUserStatus;
using InnoShop.Domain.Entities;
using Moq;
using Xunit;

namespace InnoShop.UserApplication.UnitTests.Features.Users.Commands.UpdateUserStatus
{
    public class UpdateUserStatusCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IProductApiClient> _productApiClientMock;
        private readonly UpdateUserStatusCommandHandler _handler;

        public UpdateUserStatusCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _productApiClientMock = new Mock<IProductApiClient>();
            _handler = new UpdateUserStatusCommandHandler(_userRepositoryMock.Object, _productApiClientMock.Object);
        }

        [Fact]
        public async Task Handle_Should_UpdateStatus_And_CallProductApi()
        {
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Name = "User", Email = "e", Role = "User", PasswordHash = "h", IsActive = true };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            var command = new UpdateUserStatusCommand { UserId = userId, IsActive = false };

            await _handler.Handle(command, CancellationToken.None);

            _userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<User>(u => u.IsActive == false)), Times.Once);

            _productApiClientMock.Verify(c => c.SetUserProductsVisibilityAsync(userId, false), Times.Once);
        }
    }
}