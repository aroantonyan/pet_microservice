using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Applications.Products.Commands;
using ProductService.Applications.Products.Queries;
using ProductService.Dto;

namespace ProductService.Controllers;

[ApiExplorerSettings(GroupName = "v1")]
[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route("api/[controller]")]
public class ProductController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProductInfo([FromQuery] string productName)
    {
        var result = await mediator.Send(new GetProductInfoQuery(productName));
        if (result.Data == null) return NotFound(result.ErrorMessage);
        return Ok(result.Data);
    }


    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto product)
    {
        var result = await mediator.Send(new CreateProductCommand(product));
        if (result.IsSuccess) return Ok("product is created");
        return BadRequest("product is not created");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProduct([FromQuery] string productName)
    {
        var result = await mediator.Send(new DeleteProductCommand(productName));
        if (result.IsSuccess) return Ok("product is deleted");
        return BadRequest("product is not deleted");
    }
}