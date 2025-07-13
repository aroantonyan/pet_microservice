using MediatR;
using ProductService.Dto;
using ProductService.Dto.ProductDto;

namespace ProductService.Applications.Products.Queries;

public record GetProductInfoQuery(string ProductName) : IRequest<RequestResponseDto<RequestResponseDto>>;