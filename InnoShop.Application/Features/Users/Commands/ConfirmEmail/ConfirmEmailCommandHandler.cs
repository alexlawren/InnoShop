using InnoShop.Application.Contracts.Persistence;
using InnoShop.Shared.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace InnoShop.Application.Features.Users.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration; 

        public ConfirmEmailCommandHandler(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user is null ||
                user.EmailConfirmationToken != request.Token ||
                user.ConfirmationTokenExpiry < DateTime.UtcNow)
            {
                throw new InvalidLinkException("Ссылка для подтверждения недействительна или устарела.");
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;
            user.ConfirmationTokenExpiry = null;

            await _userRepository.UpdateAsync(user);

            var clientLoginUrl = _configuration["ClientSettings:BaseUrl"];
            return clientLoginUrl;
        }
    }
}