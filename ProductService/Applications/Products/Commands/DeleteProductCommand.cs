using MediatR;
using ProductService.Dto;
using ProductService.Models;

namespace ProductService.Applications.Products;

public class DeleteProductCommand(string productName) : IRequest<ProductResult<Product>>;