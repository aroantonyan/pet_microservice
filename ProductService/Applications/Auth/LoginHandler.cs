using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductService.Dto.AuthDto;
using ProductService.Dto.ProductDto;
using ProductService.Models;
using ProductService.Services.TokenGenerator;

namespace ProductService.Applications.Auth;

public sealed class LoginHandler(
    UserManager<User> userManager,
    AccessTokenGenerator accessTokenGenerator,
    RefreshTokenService refreshTokenService)
    : IRequestHandler<LoginCommand, RequestResponseDto<RefreshAccessTokenDto>>
{
    public async Task<RequestResponseDto<RefreshAccessTokenDto>> Handle(
        LoginCommand request,
        CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(request.LoginDto.Email);
        var valid = user is not null && await userManager.CheckPasswordAsync(user, request.LoginDto.Password);

        if (!valid)
            return new RequestResponseDto<RefreshAccessTokenDto> { IsSuccess = false, ErrorMessage = "Invalid credentials" };

        var accessToken = accessTokenGenerator.GenerateAccessToken(user!);
        var existingRefreshToken = await refreshTokenService.GetRefreshTokenAsync(user!.Id);
        if (existingRefreshToken is not null && existingRefreshToken.ExpiryDate > DateTime.UtcNow)
        {
            return new RequestResponseDto<RefreshAccessTokenDto> { IsSuccess = true, Data = new RefreshAccessTokenDto()
            {
                AccessToken = accessToken,
                RefreshToken = existingRefreshToken.Token
            } };
            
        }

        if (existingRefreshToken is not null && existingRefreshToken.ExpiryDate < DateTime.UtcNow)
        {
            await refreshTokenService.RevokeRefreshTokenAsync(existingRefreshToken);
            
            var newRefreshToken = await refreshTokenService.CreateRefreshTokenAsync(user!.Id);
            
            await refreshTokenService.SaveRefreshTokenAsync(newRefreshToken);
            
            return new RequestResponseDto<RefreshAccessTokenDto> { IsSuccess = true, Data = new RefreshAccessTokenDto()
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token
            } };
        }
        return new RequestResponseDto<RefreshAccessTokenDto> { IsSuccess = false ,ErrorMessage = "User initially didn't have the refresh token"};


    }
}