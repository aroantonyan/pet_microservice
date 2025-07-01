using Microsoft.AspNetCore.Mvc;
using ProductService.Dto;
using ProductService.Models;

namespace ProductService.Interfaces;

public interface IProductInterface
{
    public Task<ProductResult<ProductResultDto>> GetProductInfo(string productName);
    public Task<ProductResult<Product>> CrateProduct(CreateProductDto dto);
}