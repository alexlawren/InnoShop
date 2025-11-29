using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.ProductApplication.Features.Products.Commands.UpdateProductsVisibility
{
    public class UpdateProductsVisibilityCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public bool IsVisible { get; set; }
    }
}
