using InnoShop.ProductApplication.Contracts.Persistence;
using InnoShop.ProductApplication.Contracts.Services;
using InnoShop.Shared.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.ProductApplication.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateProductCommandHandler(IProductRepository productRepository, ICurrentUserService currentUserService)
        {
            _productRepository = productRepository;
            _currentUserService = currentUserService; 
        }

        public async Task<Unit> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var productToUpdate = await _productRepository.GetByIdAsync(command.ProductId);
            var currentUserId = _currentUserService.UserGuid;

            if (productToUpdate is null)
            {
                throw new NotFoundException($"Продукт с ID {command.ProductId} не найден.");
            }

            if (productToUpdate.UserId != currentUserId.Value)
            {
                throw new ForbiddenException("У вас нет прав для редактирования этого продукта.");
            }

            var cleanName = command.Name.Trim();
            var cleanDescription = command.Description.Trim();

            productToUpdate.Name = cleanName;
            productToUpdate.Description = cleanDescription;
            productToUpdate.Price = command.Price;
            productToUpdate.IsAvailable = command.IsAvailable;

            await _productRepository.UpdateAsync(productToUpdate);

            return Unit.Value;
        }
    }
}
