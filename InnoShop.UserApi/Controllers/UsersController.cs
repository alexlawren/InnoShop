using InnoShop.Application.DTOs.Users;
using InnoShop.Application.Features.Users.Commands.ChangeUserRole;
using InnoShop.Application.Features.Users.Commands.ConfirmEmail;
using InnoShop.Application.Features.Users.Commands.CreateUser;
using InnoShop.Application.Features.Users.Commands.DeleteUser;
using InnoShop.Application.Features.Users.Commands.ForgotPassword;
using InnoShop.Application.Features.Users.Commands.ResetPassword;
using InnoShop.Application.Features.Users.Commands.UpdateUserStatus;
using InnoShop.Application.Features.Users.Queries.GetAllUsers;
using InnoShop.Application.Features.Users.Queries.Login;
using InnoShop.ProductApplication.DTOs;
using InnoShop.ProductApplication.Features.Products.Queries.SearchProducts;
using InnoShop.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace InnoShop.UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var userId = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateUser), new { Id = userId }, userId);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginQuery guery)
        {

            var token = await _mediator.Send(guery);
            return Ok(new { Token = token });
        }

        [HttpGet("profile")]
        [Authorize]
        public IActionResult GetUserProfile()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userEmail is null || userId is null)
            {
                return Unauthorized();
            }

            return Ok(new { id = userId, Email = userEmail});
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedResult<UserDto>>> GetAllUsers([FromQuery] GetAllUsersQuery query)
        {
            var pagedResult = await _mediator.Send(query);
            return Ok(pagedResult);
        }

        [HttpPatch("{userId}/status")] 
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserStatus(Guid userId, [FromBody] UpdateUserStatusCommand command)
        {
            if (userId != command.UserId)
            {
                return BadRequest("ID пользователя в маршруте и в теле запроса не совпадают.");
            }

            await _mediator.Send(command);

            return NoContent(); 
        }

        [HttpDelete("{userId}")] 
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            await _mediator.Send(new DeleteUserCommand(userId));

            return NoContent();
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command)
        {
            var clientRedirectUrl = await _mediator.Send(command);

            return Redirect(clientRedirectUrl);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok("Если пользователь с таким email существует, мы отправили инструкцию по сбросу пароля на вашу почту.");
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok("Пароль успешно изменен.");
        }

        [HttpPatch("{userId}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeUserRole(Guid userId, [FromBody] ChangeUserRoleCommand command)
        {
            if (userId != command.UserId)
            {
                return BadRequest("ID пользователя в URL и в теле запроса не совпадают.");
            }

            await _mediator.Send(command);

            return NoContent();
        }
    }
}
