using InnoShop.ProductApplication.Contracts.Persistence;
using InnoShop.ProductApplication.Contracts.Services;
using InnoShop.ProductDomain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.ProductApplication.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreateProductCommandHandler(IProductRepository productRepository, ICurrentUserService currentUserService)
        {
            _productRepository = productRepository;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserGuid;

            if (userId is null)
            {
                throw new Exception("Пользователь не аутентифицирован");
            }

            var cleanName = request.Name.Trim();
            var cleanDescription = request.Description.Trim();

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = cleanName,
                Description = cleanDescription,
                Price = request.Price,
                IsAvailable = true,
                UserId = userId.Value,
                CreatedAt = DateTime.UtcNow
            };

            await _productRepository.AddAsync(product);

            return product.Id;
        }
    }
}
