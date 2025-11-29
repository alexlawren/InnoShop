using FluentAssertions;
using InnoShop.Application.Contracts.Infrastructure;
using InnoShop.Application.Contracts.Persistence;
using InnoShop.Application.Features.Users.Commands.CreateUser;
using InnoShop.Domain.Entities;
using Moq;
using Xunit;

namespace InnoShop.UserApplication.UnitTests.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;

        public CreateUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _emailServiceMock = new Mock<IEmailService>();
        }

        [Fact]
        public async Task Handle_Should_CreateUser_And_SendEmail_When_DataIsValid()
        {
            var command = new CreateUserCommand
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "StrongPassword123!"
            };

            _userRepositoryMock
                .Setup(repo => repo.GetByEmailAsync(command.Email))
                .ReturnsAsync((User?)null);

            var handler = new CreateUserCommandHandler(_userRepositoryMock.Object, _emailServiceMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);


            result.Should().NotBeEmpty();

            _userRepositoryMock.Verify(repo => repo.AddAsync(It.Is<User>(u =>
                u.Name == command.Name &&
                u.Email == command.Email &&
                u.Role == "User" &&
                u.PasswordHash != command.Password 
            )), Times.Once);

            _emailServiceMock.Verify(email => email.SendConfirmationEmailAsync(
                command.Email,
                command.Name,
                It.IsAny<string>() 
            ), Times.Once);
        }
    }
}