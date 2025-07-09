using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using ProductService.Data;
using ProductService.Dto;
using ProductService.Interfaces;
using ProductService.Models;
using PriceContracts;
using ProductService.Delegates;
using ProductService.Events;

namespace ProductService.Services;

public class ProductService(
    AppDbContext context,
    PriceService.PriceServiceClient priceServiceClient,
    IDistributedCache cache) : IProductInterface
{

    public async Task<ProductResult<ProductResultDto>> GetProductInfo(string productName)
    {
        Console.WriteLine($"[SERVICE] Requested: \"{productName}\"");

        var product = await context.Products
            .FirstOrDefaultAsync(p => p.ProductName.ToLower() == productName.ToLower());

        Console.WriteLine(product is null
            ? "[SERVICE] Product NOT FOUND in DB"
            : $"[SERVICE] Found product (Id={product.ProductName}, PriceId={product.PriceId})");

        if (product is null)
            return new ProductResult<ProductResultDto>
            {
                IsSuccess = false,
                ErrorMessage = "Product not found"
            };
        
        var cacheKey = $"price:{product.PriceId}";
        var cached = cache.GetString(cacheKey);
        
        

        if (cached is not null)
        {
            Console.WriteLine("bkabkabka");
            return new ProductResult<ProductResultDto>
            {
                IsSuccess = true,
                Data = new ProductResultDto()
                {
                    ProductPrice = double.Parse(cached, CultureInfo.InvariantCulture),
                    ProductDescription = product.ProductDescription,
                    ProductName = product.ProductName,
                    
                }
            };
        }

        Console.WriteLine("dsbcbshcsdj");
        LogDelegate.LogToConsole("a request to get a product was sent");
        ProductEvents.LogMessage("a request to get a product was sent");

        var priceResponse = await priceServiceClient.GetPriceByIdAsync(new GetPriceByIdRequest
        {
            PriceId = product.PriceId.ToString()
        });
        
        
        if (priceResponse.Found)
        {
            await cache.SetStringAsync(cacheKey, priceResponse.Value.ToString(CultureInfo.InvariantCulture), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
        }

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

    public async Task<ProductResult<Product>> CrateProduct(CreateProductDto dto)
    {
        Guid priceId = Guid.NewGuid();
        var product = new Product()
        {
            ProductDescription = dto.ProductDescription,
            ProductName = dto.ProductName,
            PriceId = priceId
        };
        await priceServiceClient.CreatePriceAsync(new CreatePriceRequest
        {
            PriceId = priceId.ToString(),
            Value = dto.ProductPrice,
            Currency = "USD"
        });


        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        LogDelegate.LogToConsole("A request to create a product was sent");

        return new ProductResult<Product>() { IsSuccess = true };
    }

    public async Task<ProductResult<Product>> DeleteProduct(string productName)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.ProductName == productName);
        if (product is null)
            return new ProductResult<Product>() { IsSuccess = false, ErrorMessage = "Product not found" };
        var result = await priceServiceClient.DeletePriceAsync(new DeletePriceRequest
        {
            PriceId = product.PriceId.ToString()
        });
        if (!result.Success) ProductEvents.LogMessage($"PriceId{product.PriceId} was not deleted from price db");
        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return new ProductResult<Product>() { IsSuccess = true };
    }
}