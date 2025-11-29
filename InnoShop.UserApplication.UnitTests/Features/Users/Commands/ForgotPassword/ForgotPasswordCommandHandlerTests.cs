using FluentAssertions;
using InnoShop.Application.Contracts.Infrastructure;
using InnoShop.Application.Contracts.Persistence;
using InnoShop.Application.Features.Users.Commands.ForgotPassword;
using InnoShop.Domain.Entities;
using Moq;
using Xunit;

namespace InnoShop.UserApplication.UnitTests.Features.Users.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly ForgotPasswordCommandHandler _handler;

        public ForgotPasswordCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            _handler = new ForgotPasswordCommandHandler(_userRepositoryMock.Object, _emailServiceMock.Object);
        }

        [Fact]
        public async Task Handle_Should_GenerateTokenAndSendEmail_When_UserExists()
        {
            var email = "test@example.com";
            var user = new User { Id = Guid.NewGuid(), Name = "Test", Email = email, Role = "User", PasswordHash = "hash" };

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);

            var command = new ForgotPasswordCommand { Email = email };

            await _handler.Handle(command, CancellationToken.None);

            _userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<User>(u =>
                u.PasswordResetToken != null &&
                u.ResetTokenExpiry > DateTime.UtcNow
            )), Times.Once);

            _emailServiceMock.Verify(e => e.SendPasswordResetEmailAsync(email, user.Name, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_DoNothing_When_UserDoesNotExist()
        {
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
            var command = new ForgotPasswordCommand { Email = "unknown@example.com" };

            await _handler.Handle(command, CancellationToken.None);

            _userRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
            _emailServiceMock.Verify(e => e.SendPasswordResetEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}