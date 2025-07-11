using MediatR;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ProductService.Dto;

namespace ProductService.Applications.Products;

public record GetProductInfoQuery(string productName) : IRequest<ProductResult<ProductResultDto>>;