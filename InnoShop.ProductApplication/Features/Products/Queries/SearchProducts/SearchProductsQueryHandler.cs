using AutoMapper;
using InnoShop.ProductApplication.Contracts.Persistence;
using InnoShop.ProductApplication.DTOs;
using InnoShop.ProductDomain.Entities;
using InnoShop.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.ProductApplication.Features.Products.Queries.SearchProducts
{
    public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, PagedResult<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public SearchProductsQueryHandler (IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<ProductDto>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
        {
            var pagedProducts = await _productRepository.SearchAsync(
                request.SearchText,
                request.MinPrice,
                request.MaxPrice,
                request.IsAvailable,
                request.PageNumber,
                request.PageSize);

            var productDtos = _mapper.Map<List<ProductDto>>(pagedProducts);

            return new PagedResult<ProductDto>(
                productDtos,
                pagedProducts.TotalCount,
                pagedProducts.PageSize,
                pagedProducts.CurrentPage);
        }
    }
}
