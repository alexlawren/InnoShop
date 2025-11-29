using BCrypt.Net;
using InnoShop.Application.Contracts.Infrastructure;
using InnoShop.Application.Contracts.Persistence;
using InnoShop.Domain;
using InnoShop.Domain.Entities;
using InnoShop.Shared.Exceptions;
using MediatR;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;

namespace InnoShop.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        public CreateUserCommandHandler(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var confirmationToken = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");

            var cleanEmail = request.Email.Trim();
            var cleanName = request.Name.Trim();

            var existingUser = await _userRepository.GetByEmailAsync(cleanEmail);
            if (existingUser != null)
            {
                throw new ConflictException($"Пользователь с email '{cleanEmail}' уже существует.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = cleanName, 
                Email = cleanEmail, 
                Role = "User",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, 12),
                IsActive = true,
                IsEmailConfirmed = false,
                EmailConfirmationToken = confirmationToken,
                ConfirmationTokenExpiry = DateTime.UtcNow.AddHours(24)
            };

            await _userRepository.AddAsync(user);
            await _emailService.SendConfirmationEmailAsync(user.Email, user.Name, user.EmailConfirmationToken!);

            return user.Id;
        }
    }
}
