using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Application.Features.Users.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<string>
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
    }
}
