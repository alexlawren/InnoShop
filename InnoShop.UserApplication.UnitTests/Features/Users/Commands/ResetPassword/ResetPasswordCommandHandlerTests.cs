using InnoShop.Application.Contracts.Persistence;
using InnoShop.Application.Features.Users.Commands.ResetPassword;
using InnoShop.Domain.Entities;
using Moq;
using Xunit;

namespace InnoShop.UserApplication.UnitTests.Features.Users.Commands.ResetPassword
{
    public class ResetPasswordCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly ResetPasswordCommandHandler _handler;

        public ResetPasswordCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new ResetPasswordCommandHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ResetPassword_When_TokenIsValid()
        {
            var command = new ResetPasswordCommand
            {
                Email = "test@test.com",
                Token = "valid_token",
                NewPassword = "new_password"
            };

            var user = new User
            {
                Email = command.Email,
                PasswordResetToken = "valid_token",
                ResetTokenExpiry = DateTime.UtcNow.AddHours(1), 
                Name = "Test",
                Role = "User",
                PasswordHash = "old_hash"
            };

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email)).ReturnsAsync(user);

            await _handler.Handle(command, CancellationToken.None);

            _userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<User>(u =>
                u.PasswordResetToken == null && 
                u.PasswordHash != "old_hash"    
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_TokenExpired()
        {
            var command = new ResetPasswordCommand { Email = "t", Token = "t", NewPassword = "p" };
            var user = new User
            {
                Email = "t",
                PasswordResetToken = "t",
                ResetTokenExpiry = DateTime.UtcNow.AddHours(-1),
                Name = "t",
                Role = "u",
                PasswordHash = "h"
            };

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email)).ReturnsAsync(user);

            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}