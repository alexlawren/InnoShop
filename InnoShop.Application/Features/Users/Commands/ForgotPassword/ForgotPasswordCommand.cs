using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Application.Features.Users.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest
    {
        public required string Email { get; set; }
    }
}
