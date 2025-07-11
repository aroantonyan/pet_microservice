using MediatR;
using ProductService.Dto;
using ProductService.Interfaces;

namespace ProductService.Applications.Products;

public class GetProductInfoHandler(IProductInterface productService)
    : IRequestHandler<GetProductInfoQuery, ProductResult<ProductResultDto>>
{
    public async Task<ProductResult<ProductResultDto>> Handle(GetProductInfoQuery query,
        CancellationToken cancellationToken)
    {
        var result = await productService.GetProductInfo(query.productName);
        return result;
    }
}