using InnoShop.ProductApplication.DTOs;
using InnoShop.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.ProductApplication.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<PagedResult<ProductDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
