using AutoMapper;
using InnoShop.ProductApplication.Contracts.Persistence;
using InnoShop.ProductApplication.DTOs;
using InnoShop.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.ProductApplication.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PagedResult<ProductDto>>
    {
        readonly IProductRepository _productRepository;
        readonly IMapper _mapper;

        public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;   
        }

        public async Task<PagedResult<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var pagedProducts = await _productRepository.GetAllAsync(request.PageNumber, request.PageSize);

            var productDtos = _mapper.Map<List<ProductDto>>(pagedProducts);

            return new PagedResult<ProductDto>(
                productDtos,
                pagedProducts.TotalCount,
                pagedProducts.PageSize,
                pagedProducts.CurrentPage);
        }
    }
}
