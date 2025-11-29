using InnoShop.Application.Contracts.Persistence;
using InnoShop.Application.Features.Users.Commands.ChangeUserRole;
using InnoShop.Domain.Entities;
using InnoShop.Shared.Exceptions; 
using Moq;
using Xunit;
using MediatR;

namespace InnoShop.UserApplication.UnitTests.Features.Users.Commands.ChangeUserRole
{
    public class ChangeUserRoleCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly ChangeUserRoleCommandHandler _handler;

        public ChangeUserRoleCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new ChangeUserRoleCommandHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_UpdateRole_When_UserExists()
        {
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Name = "User", Email = "e", Role = "User", PasswordHash = "h" };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            var command = new ChangeUserRoleCommand { UserId = userId, NewRole = "Admin" };

            await _handler.Handle(command, CancellationToken.None);

            _userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<User>(u => u.Role == "Admin")), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFound_When_UserDoesNotExist()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);
            var command = new ChangeUserRoleCommand { UserId = Guid.NewGuid(), NewRole = "Admin" };

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}