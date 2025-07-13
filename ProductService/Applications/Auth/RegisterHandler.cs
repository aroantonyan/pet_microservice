using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductService.Dto;
using ProductService.Dto.ProductDto;

namespace ProductService.Applications.Auth;

public class RegisterHandler(UserManager<IdentityUser> userManager) : IRequestHandler<RegisterCommand, RequestResponseDto<string>>
{
    public async Task<RequestResponseDto<string>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var alreadyRegistered = await userManager.FindByEmailAsync(command.RegisterDto.Email);
        if (alreadyRegistered != null) return new RequestResponseDto<string>(){IsSuccess = false, ErrorMessage = "User with provided email already exists."};
        var user = new IdentityUser()
        {
            UserName = command.RegisterDto.Username,
            Email = command.RegisterDto.Email,
        };
        var result = await userManager.CreateAsync(user, command.RegisterDto.Password);
        return !result.Succeeded ? new RequestResponseDto<string>(){IsSuccess = false,ErrorMessage = "Problems with creating user"}
            : new RequestResponseDto<string>(){IsSuccess = true, Data = "User created"};
    }
}