using AutoMapper;
using FluentAssertions;
using InnoShop.ProductApplication.Contracts.Persistence;
using InnoShop.ProductApplication.DTOs;
using InnoShop.ProductApplication.Features.Products.Queries.GetAllProducts;
using InnoShop.ProductDomain.Entities;
using InnoShop.Shared.Models;
using Moq;
using Xunit;

namespace InnoShop.ProductApplication.UnitTests.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllProductsQueryHandler _handler;

        public GetAllProductsQueryHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllProductsQueryHandler(_productRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnPagedResult_WithCorrectData()
        {
            var query = new GetAllProductsQuery
            {
                PageNumber = 1,
                PageSize = 10
            };

            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Laptop", Description = "Gaming Laptop", Price = 1500, IsAvailable = true, UserId = Guid.NewGuid() },
                new Product { Id = Guid.NewGuid(), Name = "Mouse", Description = "Wireless Mouse", Price = 50, IsAvailable = true, UserId = Guid.NewGuid() }
            };

            var pagedList = new PagedList<Product>(products, count: 2, pageNumber: 1, pageSize: 10);

            var productDtos = new List<ProductDto>
            {
                new ProductDto { Id = products[0].Id, Name = "Laptop", Description = "Gaming Laptop", Price = 1500, IsAvailable = true, UserId = products[0].UserId },
                new ProductDto { Id = products[1].Id, Name = "Mouse", Description = "Wireless Mouse", Price = 50, IsAvailable = true, UserId = products[1].UserId }
            };

            _productRepositoryMock
                .Setup(r => r.GetAllAsync(query.PageNumber, query.PageSize))
                .ReturnsAsync(pagedList);

            _mapperMock
                .Setup(m => m.Map<List<ProductDto>>(It.IsAny<List<Product>>()))
                .Returns(productDtos);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.TotalCount.Should().Be(2);
            result.Items.Should().HaveCount(2);
            result.PageSize.Should().Be(10);

            result.Items[0].Name.Should().Be("Laptop");
            result.Items[0].Price.Should().Be(1500);

            _productRepositoryMock.Verify(r => r.GetAllAsync(query.PageNumber, query.PageSize), Times.Once);
            _mapperMock.Verify(m => m.Map<List<ProductDto>>(pagedList), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyResult_When_RepositoryReturnsEmpty()
        {
            var query = new GetAllProductsQuery { PageNumber = 1, PageSize = 10 };

            var products = new List<Product>();
            var pagedList = new PagedList<Product>(products, count: 0, pageNumber: 1, pageSize: 10);
            var productDtos = new List<ProductDto>();

            _productRepositoryMock
                .Setup(r => r.GetAllAsync(query.PageNumber, query.PageSize))
                .ReturnsAsync(pagedList);

            _mapperMock
                .Setup(m => m.Map<List<ProductDto>>(It.IsAny<List<Product>>()))
                .Returns(productDtos);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.TotalCount.Should().Be(0);
            result.Items.Should().BeEmpty();
        }
    }
}