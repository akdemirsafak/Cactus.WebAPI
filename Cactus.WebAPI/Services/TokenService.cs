using Cactus.WebAPI.Common;
using Cactus.WebAPI.Entities;
using Cactus.WebAPI.Modals.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Cactus.WebAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public Task<AuthResponse> CreateTokenAsync(AppUser user)
        {
            var jwt = _config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["SecretKey"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email)
        };

            var accessTokenMinutes = int.Parse(jwt["AccessTokenMinutes"]);

            var expireAt = DateTime.UtcNow.AddMinutes(accessTokenMinutes);

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: expireAt,
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Task.FromResult(new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = GenerateRefreshToken(),
                AccessTokenExpireAt = expireAt
            });
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                   + Guid.NewGuid().ToString("N");
        }
    }
}