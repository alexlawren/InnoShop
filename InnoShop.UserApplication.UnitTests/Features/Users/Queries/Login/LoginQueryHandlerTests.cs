using FluentAssertions;
using InnoShop.Application.Features.Users.Queries.Login;
using InnoShop.Application.Contracts.Persistence;
using InnoShop.Domain.Entities;
using InnoShop.Shared.Exceptions; 
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace InnoShop.UserApplication.UnitTests.Features.Users.Queries.Login
{
    public class LoginQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly LoginQueryHandler _handler;

        public LoginQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _configMock = new Mock<IConfiguration>();

            _configMock.Setup(c => c["JwtSettings:Secret"]).Returns("super_secret_key_for_tests_at_least_32_chars");
            _configMock.Setup(c => c["JwtSettings:Issuer"]).Returns("TestIssuer");
            _configMock.Setup(c => c["JwtSettings:Audience"]).Returns("TestAudience");
            _configMock.Setup(c => c["JwtSettings:ExpiryMinutes"]).Returns("60");

            _handler = new LoginQueryHandler(_userRepositoryMock.Object, _configMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnToken_When_CredentialsAreCorrect_And_EmailIsConfirmed()
        {
            var password = "password123";
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                PasswordHash = passwordHash,
                Role = "User",
                Name = "Test",
                IsEmailConfirmed = true 
            };

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(user);

            var query = new LoginQuery { Email = user.Email, Password = password };

            var token = await _handler.Handle(query, CancellationToken.None);

            token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Handle_Should_ThrowAuthException_When_EmailIsNotConfirmed()
        {
            var password = "password123";
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Email = "test@test.com",
                Role = "User",
                Name = "Test",
                PasswordHash = passwordHash,
                IsEmailConfirmed = false 
            };

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(user);
            var query = new LoginQuery { Email = user.Email, Password = password };
            await Assert.ThrowsAsync<AuthException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ThrowAuthException_When_PasswordIsWrong()
        {
            var user = new User
            {
                Email = "test@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("correct_password"),
                Role = "User",
                Name = "Test"
            };

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(user);

            var query = new LoginQuery { Email = user.Email, Password = "WRONG_PASSWORD" };

            await Assert.ThrowsAsync<AuthException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}