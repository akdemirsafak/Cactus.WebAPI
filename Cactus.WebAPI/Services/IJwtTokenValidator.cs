using System.Security.Claims;

namespace Cactus.WebAPI.Services
{
    public interface IJwtTokenValidator
    {
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
