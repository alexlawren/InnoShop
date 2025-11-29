using InnoShop.Application.DTOs.Users;
using InnoShop.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<PagedResult<UserDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
