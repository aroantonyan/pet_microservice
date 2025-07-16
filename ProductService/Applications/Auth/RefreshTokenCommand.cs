using MediatR;
using ProductService.Dto.AuthDto;
using ProductService.Dto.ProductDto;

namespace ProductService.Applications.Auth;

public record RefreshTokenCommand(RefreshTokenRequestDto RefreshTokenRequestDto):IRequest<RequestResponseDto<RefreshTokenResponseDto>>;