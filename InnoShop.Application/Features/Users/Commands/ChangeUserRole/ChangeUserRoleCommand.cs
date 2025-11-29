using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Application.Features.Users.Commands.ChangeUserRole
{
    public class ChangeUserRoleCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public required string NewRole { get; set; }
    }
}
