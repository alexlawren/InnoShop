using FluentAssertions;
using InnoShop.Application.Contracts.Persistence;
using InnoShop.Application.Features.Users.Commands.DeleteUser;
using InnoShop.Domain.Entities;
using Moq;
using Xunit; 


namespace InnoShop.UserApplication.UnitTests.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly DeleteUserCommandHandler _handler;

        public DeleteUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new DeleteUserCommandHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_DeleteUser_When_UserExists()
        {
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Name = "Del", Email = "d", Role = "u", PasswordHash = "p" };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            var command = new DeleteUserCommand(userId);

            await _handler.Handle(command, CancellationToken.None);

            _userRepositoryMock.Verify(r => r.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_UserNotFound()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);
            var command = new DeleteUserCommand(Guid.NewGuid());

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}