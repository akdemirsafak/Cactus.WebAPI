using Cactus.WebAPI.Modals.Auth;


namespace Cactus.WebAPI.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request, ClientInfo clientInfo);

        Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, ClientInfo clientInfo);
        Task<bool> ChangePasswordAsync(string userId, ChangePasswordRequest request);

        Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request);

        Task<bool> ResetPasswordAsync(ResetPasswordRequest request);
    }
}
