using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.ProductApplication.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
    }
}
