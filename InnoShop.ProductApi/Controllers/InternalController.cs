using InnoShop.ProductApplication.Features.Products.Commands.UpdateProductsVisibility;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InnoShop.ProductApi.Controllers
{
    [Route("api/internal/users")]
    [ApiController]
    public class InternalController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InternalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPatch("{userId}/products-visibility")]
        public async Task<ActionResult> UpdateProductsUpdateProductsVisibility(Guid userId, [FromBody] UpdateProductsVisibilityCommand command)
        {
            if(userId != command.UserId)
            {
                return BadRequest();
            }

            await _mediator.Send(command);
            return NoContent();
        }
    }
}
