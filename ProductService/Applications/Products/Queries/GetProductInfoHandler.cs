using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using PriceContracts;
using ProductService.Data;
using ProductService.Delegates;
using ProductService.Dto;
using ProductService.Dto.ProductDto;
using ProductService.Events;

namespace ProductService.Applications.Products.Queries;

public class GetProductInfoHandler(
    AppDbContext context,
    IDistributedCache cache,
    PriceService.PriceServiceClient priceServiceClient)
    : IRequestHandler<GetProductInfoQuery, RequestResponseDto<RequestResponseDto>>
{
    public async Task<RequestResponseDto<RequestResponseDto>> Handle(GetProductInfoQuery query,
        CancellationToken cancellationToken)
    {
        var product = await context.Products
            .FirstOrDefaultAsync(p => p.ProductName == query.ProductName, cancellationToken);
        if (product is null)
            return new RequestResponseDto<RequestResponseDto>
            {
                IsSuccess = false,
                ErrorMessage = "Product not found"
            };
        var cacheKey = $"price:{product.PriceId}";
        var cached = await cache.GetStringAsync(cacheKey, cancellationToken);

        if (cached is not null)
        {
            return new RequestResponseDto<RequestResponseDto>
            {
                IsSuccess = true,
                Data = new RequestResponseDto()
                {
                    ProductPrice = double.Parse(cached, CultureInfo.InvariantCulture),
                    ProductDescription = product.ProductDescription,
                    ProductName = product.ProductName,
                }
            };
        }

        LogDelegate.LogToConsole("a request to get a product was sent");
        ProductEvents.LogMessage("a request to get a product was sent");
        var priceResponse = await priceServiceClient.GetPriceByIdAsync(new GetPriceByIdRequest
            { PriceId = product.PriceId.ToString() }, cancellationToken: cancellationToken);
        if (priceResponse.Found)
        {
            await cache.SetStringAsync(cacheKey, priceResponse.Value.ToString(CultureInfo.InvariantCulture),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                },cancellationToken);
            return new RequestResponseDto<RequestResponseDto>
            {
                IsSuccess = true,
                Data = new RequestResponseDto()
                {
                    ProductPrice = double.Parse(priceResponse.Value.ToString(CultureInfo.InvariantCulture),
                        CultureInfo.InvariantCulture),
                    ProductDescription = product.ProductDescription,
                    ProductName = product.ProductName,
                }
            };
        }

        return new RequestResponseDto<RequestResponseDto>()
        {
            IsSuccess = true,
            ErrorMessage = "Price not found",
            Data = new RequestResponseDto()
            {
                ProductPrice = -1,
                ProductDescription = product.ProductDescription,
                ProductName = product.ProductName,
            }
        };
    }
}