using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Application.Features.Users.Commands.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("Пароль не может быть пустым")
                .MinimumLength(8)
                .WithMessage("Пароль должен содержать как минимуум 8 символов");
        }
    }
}
