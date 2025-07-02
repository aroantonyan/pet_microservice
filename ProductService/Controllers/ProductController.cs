using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Applications.Products;
using ProductService.Dto;
using ProductService.Interfaces;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController (IMediator mediator): ControllerBase
{
    // [HttpGet]
    // public async Task<IActionResult> GetProductInfo([FromQuery]string productName)
    // {
    //     var result =await productInterface.GetProductInfo(productName);
    //     return Ok(result);
    // }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto product)
    {
        var result = await mediator.Send(new CreateProductCommand(product));
        return Ok("product is created");
    }
}