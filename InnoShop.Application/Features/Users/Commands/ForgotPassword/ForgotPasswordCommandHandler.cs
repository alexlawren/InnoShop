using InnoShop.Application.Contracts.Infrastructure;
using InnoShop.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Application.Features.Users.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user is not null)
            {
                var resetToken = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
                user.PasswordResetToken = resetToken;
                user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);

                await _userRepository.UpdateAsync(user);

                await _emailService.SendPasswordResetEmailAsync(user.Email, user.Name, resetToken);
            }
        }
    }
}
