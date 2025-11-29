using InnoShop.ProductApplication.Contracts.Persistence;
using InnoShop.ProductApplication.Contracts.Services;
using InnoShop.Shared.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.ProductApplication.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteProductCommandHandler (IProductRepository productRepository, ICurrentUserService currentUserService)
        {
            _productRepository = productRepository;
            _currentUserService = currentUserService;
        }

        public async Task Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var productToDelete = await _productRepository.GetByIdAsync(command.ProductId); 
            var currentUserId = _currentUserService.UserGuid;

            if(productToDelete is null)
            {
                throw new NotFoundException($"Продукт с ID {command.ProductId} не найден.");
            }

            if(productToDelete.UserId !=  currentUserId)
            {
                throw new ForbiddenException("У вас нет прав для редактирования этого продукта.");
            }

            await _productRepository.DeleteAsync(productToDelete);
        }
    }
}
