using Cactus.WebAPI.Entities;
using Cactus.WebAPI.Modals.Auth;

namespace Cactus.WebAPI.Services
{
    public interface ITokenService
    {
        Task<AuthResponse> CreateTokenAsync(AppUser user);
        string GenerateRefreshToken();
    }
}
