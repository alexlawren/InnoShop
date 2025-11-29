using InnoShop.ProductApplication.DTOs;
using InnoShop.ProductApplication.Features.Products.Commands.CreateProduct;
using InnoShop.ProductApplication.Features.Products.Commands.DeleteProduct;
using InnoShop.ProductApplication.Features.Products.Commands.UpdateProduct;
using InnoShop.ProductApplication.Features.Products.Queries;
using InnoShop.ProductApplication.Features.Products.Queries.GetAllProducts;
using InnoShop.ProductApplication.Features.Products.Queries.SearchProducts;
using InnoShop.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoShop.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            var productId = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateProduct), new { productId }, productId);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ProductDto>>> GetAllProducts([FromQuery] GetAllProductsQuery query)
        {
            var products = await _mediator.Send(query);
            return Ok(products);
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult> UpdatyeProduct(Guid productId, [FromBody] UpdateProductCommand command)
        {
            command.ProductId = productId;

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteProduct(Guid productId)
        {
            await _mediator.Send(new DeleteProductCommand {ProductId = productId});
            return NoContent();
        }

        [HttpGet("search")]
        [AllowAnonymous] 
        public async Task<ActionResult<PagedResult<ProductDto>>> SearchProducts([FromQuery] SearchProductsQuery query)
        {
            var pagedResult = await _mediator.Send(query);
            return Ok(pagedResult);
        }
    }
}
