using Microsoft.AspNetCore.Identity;

namespace ProductService.Models;

public class User : IdentityUser
{
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}