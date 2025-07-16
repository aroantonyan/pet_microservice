using MediatR;
using ProductService.Dto.AuthDto;
using ProductService.Dto.ProductDto;

namespace ProductService.Applications.Auth;

public record RegisterCommand(RegisterDto RegisterDto): IRequest<RequestResponseDto<string>>;