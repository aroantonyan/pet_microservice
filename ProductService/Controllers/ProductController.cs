using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Applications.Products;
using ProductService.Applications.Products.Commands;
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
        Console.WriteLine($"[API] GET /api/Product?productName={productName}");

        var result = await mediator.Send(new GetProductInfoQuery(productName));

        if (result.Data == null)
        {
            Console.WriteLine($"[API] 404  –  {result.ErrorMessage}");
            return NotFound(result.ErrorMessage);
        }

        Console.WriteLine("[API] 200 OK");
        return Ok(result.Data);
    }


    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto product)
    {
        var result = await mediator.Send(new CreateProductCommand(product));
        if (result.IsSuccess) return Ok("product is created");
        return BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProduct([FromQuery] string productName)
    {
        var result = await mediator.Send(new DeleteProductCommand(productName));
        if (result.IsSuccess) return Ok("product is deleted");
        return BadRequest();
    }
}