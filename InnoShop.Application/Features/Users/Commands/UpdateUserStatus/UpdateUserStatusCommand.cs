using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Application.Features.Users.Commands.UpdateUserStatus
{
    public class UpdateUserStatusCommand : IRequest
    {
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
