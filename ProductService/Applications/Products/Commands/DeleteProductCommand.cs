using MediatR;
using ProductService.Dto;
using ProductService.Dto.ProductDto;
using ProductService.Models;

namespace ProductService.Applications.Products.Commands;

public class DeleteProductCommand(string productName) : IRequest<RequestResponseDto<Product>>;