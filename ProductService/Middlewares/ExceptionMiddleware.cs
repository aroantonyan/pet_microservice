using System.Net;
using System.Text.Json;
using ProductService.Dto;
using ProductService.Dto.ProductDto;
using ProductService.Services;

namespace ProductService.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, LogSenderService logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var response = new RequestResponseDto<object>
            {
                IsSuccess = false,
                ErrorMessage = "An unexpected error occurred."
            };
            var log = new LogDto
            {
                Message = "Unhandled exception",
                Level = "Error",
                Source = "ProductService",
                Timestamp = DateTime.UtcNow,
                Exception = ex.ToString(),
                Path = context.Request.Path,
                TraceId = context.TraceIdentifier
            };
            await logger.SendLogAsync(log);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}