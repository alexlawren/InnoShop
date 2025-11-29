using InnoShop.ProductApplication.DTOs;
using InnoShop.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.ProductApplication.Features.Products.Queries.SearchProducts
{
    public class SearchProductsQuery : IRequest<PagedResult<ProductDto>>
    {
        public string? SearchText {  get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsAvailable { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
