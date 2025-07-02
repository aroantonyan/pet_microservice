using MediatR;
using ProductService.Dto;
using ProductService.Interfaces;
using ProductService.Models;

namespace ProductService.Applications.Products;

public class CreateProductHandler(IProductInterface productService) : IRequestHandler<CreateProductCommand, ProductResult<Product>>
{
    public async Task<ProductResult<Product>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        return await productService.CrateProduct(command.Dto);
    }
}