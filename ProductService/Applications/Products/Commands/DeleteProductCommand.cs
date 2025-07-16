using MediatR;
using ProductService.Dto.ProductDto;
using ProductService.Models;

namespace ProductService.Applications.Products.Commands;

public record DeleteProductCommand(string ProductName) : IRequest<RequestResponseDto<Product>>;