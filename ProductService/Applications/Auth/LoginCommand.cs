using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Dto;
using ProductService.Dto.AuthDto;
using ProductService.Dto.ProductDto;

namespace ProductService.Applications.Auth;

public record LoginCommand(LoginDto LoginDto ) :  IRequest<RequestResponseDto<string>>;