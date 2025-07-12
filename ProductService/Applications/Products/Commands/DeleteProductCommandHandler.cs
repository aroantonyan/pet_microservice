using MediatR;
using Microsoft.EntityFrameworkCore;
using PriceContracts;
using ProductService.Data;
using ProductService.Dto;
using ProductService.Events;
using ProductService.Interfaces;
using ProductService.Models;

namespace ProductService.Applications.Products.Commands;

public class DeleteProductCommandHandler(AppDbContext context, PriceService.PriceServiceClient priceService) : IRequestHandler<DeleteProductCommand, ProductResult<Product>>
{
    public async Task<ProductResult<Product>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.ProductName == request.ToString(),cancellationToken);
        if (product is null)
            return new ProductResult<Product>() { IsSuccess = false, ErrorMessage = "Product not found" };
        var result = await priceService.DeletePriceAsync(new DeletePriceRequest
        {
            PriceId = product.PriceId.ToString()
        }, cancellationToken);
        if (!result.Success) ProductEvents.LogMessage($"PriceId{product.PriceId} was not deleted from price db");
        context.Products.Remove(product);
        await context.SaveChangesAsync(cancellationToken);
        return new ProductResult<Product>() { IsSuccess = true };
    }
}
