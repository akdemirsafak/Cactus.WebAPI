namespace Cactus.WebAPI.Common
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }

        public int AccessTokenMinutes { get; set; }

        public int RefreshTokenDays { get; set; }
    }
}
