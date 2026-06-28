using Cactus.WebAPI.Modals.Auth;
using Cactus.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cactus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(
            IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        private ClientInfo GetClientInfo()
        {
            return new ClientInfo
            {
                Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserAgent = Request.Headers["User-Agent"].ToString()
            };
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Modals.Auth.LoginRequest request)
        {
           // var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(); //Servisin Http bağımlılığını kaldırmak için IP adresini burada alıyoruz

            var result = await _authService.LoginAsync(request, GetClientInfo());

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Modals.Auth.RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            return Ok(result);
        }


        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var result = await _authService.ChangePasswordAsync(userId, request);

            if (!result)
                return BadRequest("Password change failed");

            return Ok("Password changed successfully");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(Modals.Auth.ForgotPasswordRequest request)
        {
            await _authService.ForgotPasswordAsync(request);
            return Ok("If email exists, reset link sent");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(Modals.Auth.ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request);

            if (!result)
                return BadRequest("Reset failed");

            return Ok("Password reset successful");
        }


    }
}
