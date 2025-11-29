using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Имя пользователя не может быть пустым.")
            .MaximumLength(50).WithMessage("Имя пользователя не может превышать 50 символов.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email не может быть пустым.")
                .EmailAddress().WithMessage("Указан некорректный формат Email адреса.");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Пароль не может быть пустым")
                .MinimumLength(8)
                .WithMessage("Пароль должен содержать как минимуум 8 символов");
        }
    }
}
