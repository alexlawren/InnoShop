using InnoShop.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public DeleteUserCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
