using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Application.Features.Users.Commands.ChangeUserRole
{
    public class ChangeUserRoleCommandValidator : AbstractValidator<ChangeUserRoleCommand>
    {
        public ChangeUserRoleCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Id пользователя обязателен");
            RuleFor(x => x.NewRole)
                .NotEmpty().WithMessage("Роль не может быть пустой")
                .Must(role => role == "Admin" || role == "User")
                .WithMessage("Недоупстимая роль. Доступные роли: 'Admin', 'User'");
        }
    }
}
