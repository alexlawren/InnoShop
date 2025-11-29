using InnoShop.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Application.Features.Users.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly IUserRepository _userRepository;

        public ResetPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user is null ||
            user.PasswordResetToken != request.Token ||
            user.ResetTokenExpiry < DateTime.UtcNow)
            {
                throw new Exception("Неверный запрос на сброс пароля.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, 12);
            user.PasswordResetToken = null;
            user.ResetTokenExpiry = null;

            await _userRepository.UpdateAsync(user);

            return Unit.Value;
        }
    }
}
