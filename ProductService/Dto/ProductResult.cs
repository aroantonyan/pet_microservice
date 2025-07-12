namespace ProductService.Dto;

public class ProductResult<T>
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
}