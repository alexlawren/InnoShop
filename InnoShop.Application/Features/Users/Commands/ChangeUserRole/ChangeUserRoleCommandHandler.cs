using InnoShop.Application.Contracts.Persistence;
using MediatR;
using InnoShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using InnoShop.Shared.Exceptions;

namespace InnoShop.Application.Features.Users.Commands.ChangeUserRole
{
    public class ChangeUserRoleCommandHandler : IRequestHandler<ChangeUserRoleCommand, Unit>
    {
        private readonly IUserRepository _userRepository;

        public ChangeUserRoleCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle (ChangeUserRoleCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);

            if (user == null)
            {
                throw new NotFoundException($"Пользователь с ID {command.UserId} не найден.");
            }

            user.Role = command.NewRole;

            await _userRepository.UpdateAsync(user);

            return Unit.Value;
        }
    }
}
