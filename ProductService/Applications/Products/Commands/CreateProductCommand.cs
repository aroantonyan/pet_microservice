using MediatR;
using ProductService.Dto;
using ProductService.Models;

namespace ProductService.Applications.Products.Commands;

public record CreateProductCommand(CreateProductDto Dto) : IRequest<ProductResult<Product>>;