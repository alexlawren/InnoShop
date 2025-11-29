using FluentAssertions;
using InnoShop.Application.Contracts.Persistence;
using InnoShop.Application.Features.Users.Commands.ConfirmEmail;
using InnoShop.Domain.Entities;
using InnoShop.Shared.Exceptions;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using System;
using Xunit;

namespace InnoShop.UserApplication.UnitTests.Features.Users.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly ConfirmEmailCommandHandler _handler;

        public ConfirmEmailCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["ClientSettings:BaseUrl"]).Returns("http://test-client.com");
            _handler = new ConfirmEmailCommandHandler(_userRepositoryMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ConfirmEmailAndReturnRedirectUrl_When_TokenIsValid()
        {
            var userEmail = "test@example.com";
            var confirmationToken = "valid_token_123";

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = userEmail,
                Name = "Test User",
                Role = "User",
                PasswordHash = "some_dummy_hash", 
                EmailConfirmationToken = confirmationToken,
                ConfirmationTokenExpiry = DateTime.UtcNow.AddHours(1),
                IsEmailConfirmed = false
            };

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(userEmail)).ReturnsAsync(user);
            var command = new ConfirmEmailCommand { Email = userEmail, Token = confirmationToken };

            var resultRedirectUrl = await _handler.Handle(command, CancellationToken.None);

            _userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<User>(u =>
                u.IsEmailConfirmed == true &&
                u.EmailConfirmationToken == null &&
                u.ConfirmationTokenExpiry == null
            )), Times.Once);
            resultRedirectUrl.Should().Be("http://test-client.com");
        }

        [Fact]
        public async Task Handle_Should_ThrowInvalidLinkException_When_TokenIsInvalid()
        {
            var userEmail = "test@example.com";

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = userEmail,
                Name = "Test User",
                Role = "User",
                PasswordHash = "some_dummy_hash",
                EmailConfirmationToken = "REAL_TOKEN",
                ConfirmationTokenExpiry = DateTime.UtcNow.AddHours(1)
            };
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(userEmail)).ReturnsAsync(user);
            var command = new ConfirmEmailCommand { Email = userEmail, Token = "WRONG_TOKEN" };

            await Assert.ThrowsAsync<InvalidLinkException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ThrowInvalidLinkException_When_TokenIsExpired()
        {
            var userEmail = "test@example.com";
            var token = "expired_token";

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = userEmail,
                Name = "Test User",
                Role = "User",
                PasswordHash = "some_dummy_hash",
                EmailConfirmationToken = token,
                ConfirmationTokenExpiry = DateTime.UtcNow.AddHours(-1)
            };

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(userEmail)).ReturnsAsync(user);
            var command = new ConfirmEmailCommand { Email = userEmail, Token = token };

            await Assert.ThrowsAsync<InvalidLinkException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}