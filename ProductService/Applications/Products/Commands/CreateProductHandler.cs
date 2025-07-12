using MediatR;
using PriceContracts;
using ProductService.Data;
using ProductService.Delegates;
using ProductService.Dto;
using ProductService.Models;

namespace ProductService.Applications.Products.Commands;

public class CreateProductHandler(PriceService.PriceServiceClient priceService,
    AppDbContext context) : IRequestHandler<CreateProductCommand, ProductResult<Product>>
{
    public async Task<ProductResult<Product>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var priceId = Guid.NewGuid();
        var product = new Product()
        {
            ProductDescription = command.Dto.ProductDescription,
            ProductName = command.Dto.ProductName,
            PriceId = priceId
        };
        await priceService.CreatePriceAsync(new CreatePriceRequest
        {
            PriceId = priceId.ToString(),
            Value = command.Dto.ProductPrice,
            Currency = "USD"
        }, cancellationToken: cancellationToken);
        
        await context.Products.AddAsync(product, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        LogDelegate.LogToConsole("A request to create a product was sent");

        return new ProductResult<Product>() { IsSuccess = true };
        
    }
}
