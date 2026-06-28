namespace Cactus.WebAPI.Modals.Auth
{
    public sealed class AuthResponse
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime AccessTokenExpireAt { get; set; }
    }
}
