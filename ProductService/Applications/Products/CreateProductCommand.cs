using MediatR;
using ProductService.Dto;
using ProductService.Models;

namespace ProductService.Applications.Products;

public record CreateProductCommand(CreateProductDto Dto) : IRequest<ProductResult<Product>>;