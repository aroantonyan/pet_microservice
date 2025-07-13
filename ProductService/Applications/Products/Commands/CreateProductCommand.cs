using MediatR;
using ProductService.Dto;
using ProductService.Dto.ProductDto;
using ProductService.Models;

namespace ProductService.Applications.Products.Commands;

public record CreateProductCommand(CreateProductDto Dto) : IRequest<RequestResponseDto<Product>>;