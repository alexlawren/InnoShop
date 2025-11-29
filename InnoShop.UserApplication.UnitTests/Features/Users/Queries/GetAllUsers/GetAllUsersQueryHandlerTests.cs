using AutoMapper;
using FluentAssertions;
using InnoShop.Application.Contracts.Persistence;
using InnoShop.Application.DTOs.Users;
using InnoShop.Application.Features.Users.Queries.GetAllUsers;
using InnoShop.Domain.Entities;
using InnoShop.Shared.Models;
using Moq;
using Xunit;

namespace InnoShop.UserApplication.UnitTests.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllUsersQueryHandler _handler;

        public GetAllUsersQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllUsersQueryHandler(_userRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnPagedResult_When_Called()
        {
            var pageNumber = 1;
            var pageSize = 10;
            var query = new GetAllUsersQuery { PageNumber = pageNumber, PageSize = pageSize };

            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), Name = "User1", Email = "1", Role = "User", PasswordHash = "p" },
                new User { Id = Guid.NewGuid(), Name = "User2", Email = "2", Role = "User", PasswordHash = "p" }
            };

            var pagedList = new PagedList<User>(users, count: 2, pageNumber: 1, pageSize: 10);

            var userDtos = new List<UserDto>
            {
                new UserDto
                {
                    Id = Guid.NewGuid(),       
                    Name = "User1",
                    Email = "user1@test.com",  
                    Role = "User",            
                    IsActive = true
                },
                new UserDto
                {
                    Id = Guid.NewGuid(),       
                    Name = "User2",
                    Email = "user2@test.com",  
                    Role = "User",            
                    IsActive = true
                }
            };

            _userRepositoryMock
                .Setup(r => r.GetAllAsync(pageNumber, pageSize))
                .ReturnsAsync(pagedList);

            _mapperMock
                .Setup(m => m.Map<List<UserDto>>(It.IsAny<List<User>>()))
                .Returns(userDtos);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.TotalCount.Should().Be(2);
            result.Items.Should().HaveCount(2);
            result.Items[0].Name.Should().Be("User1");
        }
    }
}