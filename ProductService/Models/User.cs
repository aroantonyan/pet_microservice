using Microsoft.AspNetCore.Identity;

namespace ProductService.Models;

public class User : IdentityUser
{
    public string? UserEmail { get; set; }
}