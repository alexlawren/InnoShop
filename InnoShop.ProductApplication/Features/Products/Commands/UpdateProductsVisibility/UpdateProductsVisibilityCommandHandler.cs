using InnoShop.ProductApplication.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.ProductApplication.Features.Products.Commands.UpdateProductsVisibility
{
    public class UpdateProductsVisibilityCommandHandler : IRequestHandler<UpdateProductsVisibilityCommand, Unit>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductsVisibilityCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Unit> Handle (UpdateProductsVisibilityCommand command, CancellationToken cancellationToken)
        {
            await _productRepository.SetAvailabilityForUserProductsAsync(command.UserId, command.IsVisible);

            return Unit.Value;
        }
    }
}
