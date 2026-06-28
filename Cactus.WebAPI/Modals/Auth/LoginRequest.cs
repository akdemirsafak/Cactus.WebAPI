namespace Cactus.WebAPI.Modals.Auth
{
    public sealed class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }
}
