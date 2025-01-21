using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Authentication
{
    public interface IJwtService
    {
        string GenerateToken(string userId, string userName);
        bool ValidateToken(string token, out ClaimsPrincipal principal);
    }
}
