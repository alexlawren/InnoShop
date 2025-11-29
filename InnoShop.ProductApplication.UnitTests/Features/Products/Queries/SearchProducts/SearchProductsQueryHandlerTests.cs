using AutoMapper;
using FluentAssertions;
using InnoShop.ProductApplication.Contracts.Persistence;
using InnoShop.ProductApplication.DTOs;
using InnoShop.ProductApplication.Features.Products.Queries.SearchProducts;
using InnoShop.ProductDomain.Entities;
using InnoShop.Shared.Models; 
using Moq;
using Xunit;

namespace InnoShop.ProductApplication.UnitTests.Features.Products.Queries.SearchProducts
{
    public class SearchProductsQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly SearchProductsQueryHandler _handler;

        public SearchProductsQueryHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new SearchProductsQueryHandler(_productRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnPagedResult_WithCorrectData()
        {
            var query = new SearchProductsQuery
            {
                SearchText = "Phone",
                MinPrice = 100,
                PageNumber = 1,
                PageSize = 10
            };

            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Phone 1", Description = "D", Price = 500, IsAvailable = true, UserId = Guid.NewGuid() },
                new Product { Id = Guid.NewGuid(), Name = "Phone 2", Description = "D", Price = 600, IsAvailable = true, UserId = Guid.NewGuid() }
            };

            var pagedList = new PagedList<Product>(products, count: 2, pageNumber: 1, pageSize: 10);

            var productDtos = new List<ProductDto>
            {
                new ProductDto { Id = products[0].Id, Name = "Phone 1", Description = "D", Price = 500, IsAvailable = true, UserId = products[0].UserId },
                new ProductDto { Id = products[1].Id, Name = "Phone 2", Description = "D", Price = 600, IsAvailable = true, UserId = products[1].UserId }
            };

            _productRepositoryMock
                .Setup(r => r.SearchAsync(query.SearchText, query.MinPrice, query.MaxPrice, query.IsAvailable, query.PageNumber, query.PageSize))
                .ReturnsAsync(pagedList);

            _mapperMock
                .Setup(m => m.Map<List<ProductDto>>(It.IsAny<List<Product>>()))
                .Returns(productDtos);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.TotalCount.Should().Be(2);
            result.Items.Should().HaveCount(2);
            result.Items[0].Name.Should().Be("Phone 1");
        }
    }
}