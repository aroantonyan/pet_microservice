using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductService.Dto.AuthDto;
using ProductService.Dto.ProductDto;
using ProductService.Models;
using ProductService.Services;
using ProductService.Services.TokenGenerator;

namespace ProductService.Applications.Auth;

public class RefreshTokenHandler(
    RefreshTokenService refreshTokenService,
    UserManager<User> userManager,
    AccessTokenGenerator accessTokenGenerator)
    : IRequestHandler<RefreshTokenCommand, RequestResponseDto<RefreshTokenResponseDto>>
{
    public async Task<RequestResponseDto<RefreshTokenResponseDto>> Handle(RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var principal = refreshTokenService.GetPrincipalFromExpiredToken(command.RefreshTokenRequestDto.AccessToken);
        if (principal == null)
            return new RequestResponseDto<RefreshTokenResponseDto>()
                { IsSuccess = false, ErrorMessage = "Invalid access token." };

        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        var storedToken = await refreshTokenService.GetRefreshTokenAsync(command.RefreshTokenRequestDto.RefreshToken);
        if (storedToken == null || storedToken.UserId != userId)
            return new
                RequestResponseDto<RefreshTokenResponseDto>() { IsSuccess = false, ErrorMessage = "" };

        var user = await userManager.FindByIdAsync(userId);
        var newAccessToken = accessTokenGenerator.GenerateAccessToken(user!);

        return new RequestResponseDto<RefreshTokenResponseDto>()
        {
            IsSuccess = true, Data = new RefreshTokenResponseDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = storedToken.Token,
            }
        };
    }
}