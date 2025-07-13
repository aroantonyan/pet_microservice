using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductService.Dto;
using ProductService.Dto.ProductDto;
using ProductService.Services;

namespace ProductService.Applications.Auth;

public sealed class LoginHandler(
    UserManager<IdentityUser> userManager,
    JwtTokenGenerator tokenGenerator)
    : IRequestHandler<LoginCommand, RequestResponseDto<string>>
{
    public async Task<RequestResponseDto<string>> Handle(
        LoginCommand request,
        CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(request.LoginDto.Email);
        var valid = user is not null &&
                    await userManager.CheckPasswordAsync(user, request.LoginDto.Password);

        if (!valid)
            return new RequestResponseDto<string> { IsSuccess = false, ErrorMessage = "Invalid credentials" };

        var jwt = tokenGenerator.GenerateToken(user!);
        return new RequestResponseDto<string> { IsSuccess = true, Data = jwt };
    }
}