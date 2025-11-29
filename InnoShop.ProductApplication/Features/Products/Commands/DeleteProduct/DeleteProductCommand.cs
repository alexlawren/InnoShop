using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.ProductApplication.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest
    {
        public Guid ProductId { get; set; }
    }
}
