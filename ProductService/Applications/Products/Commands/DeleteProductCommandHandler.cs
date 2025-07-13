using Grpc.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PriceContracts;
using ProductService.Data;
using ProductService.Dto;
using ProductService.Dto.ProductDto;
using ProductService.Events;
using ProductService.Models;

namespace ProductService.Applications.Products.Commands;

public class DeleteProductCommandHandler(AppDbContext context, PriceService.PriceServiceClient priceService) : IRequestHandler<DeleteProductCommand, RequestResponseDto<Product>>
{
    public async Task<RequestResponseDto<Product>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.ProductName == request.ToString(),cancellationToken);
        if (product is null)
            return new RequestResponseDto<Product>() { IsSuccess = false, ErrorMessage = "Product not found" };
        var result = await priceService.DeletePriceAsync(
            new DeletePriceRequest { PriceId = product.PriceId.ToString() },
            new CallOptions(cancellationToken: cancellationToken)); 
        if (!result.Success) ProductEvents.LogMessage($"PriceId{product.PriceId} was not deleted from price db");
        context.Products.Remove(product);
        await context.SaveChangesAsync(cancellationToken);
        return new RequestResponseDto<Product>() { IsSuccess = true };
    }
}
