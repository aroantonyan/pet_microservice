using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Applications.Products.Commands;
using ProductService.Applications.Products.Queries;
using ProductService.Dto;
using ProductService.Dto.ProductDto;
using ProductService.Services;

namespace ProductService.Controllers;

[ApiExplorerSettings(GroupName = "v1")]
[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route("api/products")]
public class ProductController(IMediator mediator, LogSenderService log) : ControllerBase
{
    [HttpGet("{productName}")]
    public async Task<IActionResult> GetProductInfo(string productName)
    {
        var result = await mediator.Send(new GetProductInfoQuery(productName));
        if (result.Data == null) return NotFound(result.ErrorMessage);
        return Ok(result.Data);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto product)
    {
        var result = await mediator.Send(new CreateProductCommand(product));
        if (result.IsSuccess) return Ok(new { message = "product is created" });
        return BadRequest(new { message = "product is not created" });
    }

    [HttpDelete("{productName}")]
    public async Task<IActionResult> DeleteProduct(string productName)
    {
        var result = await mediator.Send(new DeleteProductCommand(productName));
        if (result.IsSuccess) return Ok(new { message = "product is deleted" });
        return BadRequest(new { message = "product is not deleted" });
    }
}