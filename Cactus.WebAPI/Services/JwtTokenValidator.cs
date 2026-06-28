using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cactus.WebAPI.Services
{
    public class JwtTokenValidator: IJwtTokenValidator
    {
        private readonly IConfiguration _config;

        public JwtTokenValidator(IConfiguration config)
        {
            _config = config;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwt = _config.GetSection("Jwt");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwt["Issuer"],
                ValidAudience = jwt["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwt["SecretKey"])
                ),

                ValidateLifetime = false // refresh için kritik
            };

            var handler = new JwtSecurityTokenHandler();

            var principal = handler.ValidateToken(
                token,
                tokenValidationParameters,
                out var securityToken
            );

            if (securityToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
