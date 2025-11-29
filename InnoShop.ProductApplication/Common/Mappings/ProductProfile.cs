using AutoMapper;
using InnoShop.ProductApplication.DTOs;
using InnoShop.ProductDomain.Entities;

namespace InnoShop.ProductApplication.Common.Mappings
{
    public class ProductProfile : Profile 
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}
