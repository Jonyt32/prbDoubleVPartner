using System.Security.Claims;

namespace Application.Services.Authentication
{
    public interface IJwtService
    {
        string GenerateToken(string userId, string userName);
        bool ValidateToken(string token, out ClaimsPrincipal principal);
    }
}
