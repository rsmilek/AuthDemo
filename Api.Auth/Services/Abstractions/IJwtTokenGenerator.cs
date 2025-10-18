using Api.Auth.Models;

namespace Api.Auth.Services.Abstractions
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
    }
}
