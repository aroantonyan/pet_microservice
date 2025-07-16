using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Applications.Auth;
using ProductService.Dto.AuthDto;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await mediator.Send(new RegisterCommand(registerDto));
        if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
        return Ok("User created");
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await mediator.Send(new LoginCommand(loginDto));
        if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
        return Ok(result.Data);
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        var result = await mediator.Send(new RefreshTokenCommand(refreshTokenRequestDto));
        if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
        return Ok(result.Data);
    }
}