using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Dto;
using ProductService.Interfaces;
using ProductService.Models;
using PriceContracts;

namespace ProductService.Services;

public class ProductService(AppDbContext context, PriceService.PriceServiceClient priceServiceClient, LogSenderService logSender ) : IProductInterface
{
    
    public async Task<ProductResult<ProductResultDto>> GetProductInfo(string productName_1)
    {
        var product = await context.Products.FirstOrDefaultAsync(p=>p.ProductName == productName_1);
    
        if (product is null)
            return new ProductResult<ProductResultDto> { IsSuccess = false, ErrorMessage = "Product not found" };
        
        var priceResponse = await priceServiceClient.GetPriceByIdAsync(new GetPriceByIdRequest
        {
            PriceId = product.PriceId.ToString()
        });
        await logSender.SendLogAsync("A request to get price was sent");
        
        if (priceResponse.Found)
        {
            return new ProductResult<ProductResultDto>
            {
                IsSuccess = true,
                Data = new ProductResultDto()
                {
                    ProductPrice = priceResponse.Value,
                    ProductDescription = product.ProductDescription,
                    ProductName = product.ProductName,
                }
                
            };
        }

        return new ProductResult<ProductResultDto>
        {
            IsSuccess = true,
            Data = new ProductResultDto()
            {
                ProductPrice = null,
                ProductDescription = product.ProductDescription,
                ProductName = product.ProductName,
            },
            ErrorMessage = "price not found"
        };
    }
    public async Task<ProductResult<Product>> CrateProduct(CreateProductDto dto)
    {
        Guid priceId = Guid.NewGuid();
        var product = new Product()
        {
            ProductDescription = dto.ProductDescription,
            ProductName = dto.ProductName,
            PriceId = priceId
        };
        await priceServiceClient.CreatePriceAsync(new CreatePriceRequest {
            PriceId  = priceId.ToString(),
            Value    = dto.ProductPrice,
            Currency = "USD"
        });

        
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
        
        return new ProductResult<Product>() { IsSuccess = true };
    }
}