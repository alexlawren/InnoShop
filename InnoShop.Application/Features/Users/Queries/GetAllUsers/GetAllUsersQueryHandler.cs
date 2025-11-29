using AutoMapper;
using InnoShop.Application.Contracts.Persistence;
using InnoShop.Application.DTOs.Users;
using InnoShop.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedResult<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var pagedUsers = await _userRepository.GetAllAsync(request.PageNumber, request.PageSize);
            var userDtos = _mapper.Map<List<UserDto>>(pagedUsers);

            return new PagedResult<UserDto>(userDtos, pagedUsers.TotalCount, pagedUsers.PageSize, pagedUsers.CurrentPage);
        }
    }
}
