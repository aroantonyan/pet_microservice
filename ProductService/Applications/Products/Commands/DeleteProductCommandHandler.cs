using MediatR;
using ProductService.Dto;
using ProductService.Interfaces;
using ProductService.Models;

namespace ProductService.Applications.Products;

public class DeleteProductCommandHandler(IProductInterface productService) : IRequestHandler<DeleteProductCommand, ProductResult<Product>>
{
    public async Task<ProductResult<Product>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        return await productService.DeleteProduct(request.ToString());
    }
}