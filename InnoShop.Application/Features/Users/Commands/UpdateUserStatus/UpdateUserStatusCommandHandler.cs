using InnoShop.Application.Contracts.Infrastructure;
using InnoShop.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Application.Features.Users.Commands.UpdateUserStatus
{
    public class UpdateUserStatusCommandHandler : IRequestHandler<UpdateUserStatusCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductApiClient _productApiClient;
        public UpdateUserStatusCommandHandler(IUserRepository userRepository, IProductApiClient productApiClient) // <<< ИЗМЕНЕНИЕ
        {
            _userRepository = userRepository;
            _productApiClient = productApiClient; 
        }

        public async Task Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user is null)
            {
                throw new KeyNotFoundException($"Пользователь с ID {request.UserId} не найден.");
            }

            user.IsActive = request.IsActive;

            await _userRepository.UpdateAsync(user);

            await _productApiClient.SetUserProductsVisibilityAsync(user.Id, user.IsActive);
        }
    }
}
