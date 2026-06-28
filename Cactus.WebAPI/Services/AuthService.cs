using Cactus.WebAPI.DbContexts;
using Cactus.WebAPI.Entities;
using Cactus.WebAPI.Modals.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cactus.WebAPI.Services
{
     public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly CactusDbContext _context;
        private readonly IConfiguration _config;
        private readonly IJwtTokenValidator _tokenValidator;

        public AuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            CactusDbContext context,
            IConfiguration config,
            IJwtTokenValidator tokenValidator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _context = context;

            _config = config;
            _tokenValidator = tokenValidator;
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            var user = new AppUser
            {
                Id=Guid.NewGuid().ToString(),
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            return result.Succeeded;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request, ClientInfo clientInfo)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return null;

            var check = await _signInManager.CheckPasswordSignInAsync(
                user,
                request.Password,
                false
            );

            if (!check.Succeeded)
                return null;

            var token = await _tokenService.CreateTokenAsync(user);


            var refreshTokenDays = request.RememberMe
            ? int.Parse(_config["Jwt:RememberMeRefreshTokenDays"]!)
            : int.Parse(_config["Jwt:RefreshTokenDays"]!);

            var refreshEntity = new RefreshToken
            {
                Token = token.RefreshToken,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenDays),
                CreatedByIp = clientInfo.Ip,
                CreatedByUserAgent = clientInfo.UserAgent,
                RememberMe=request.RememberMe,
                IsRevoked = false
            };

            _context.RefreshTokens.Add(refreshEntity);
            await _context.SaveChangesAsync();

            return token;
        }


        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, ClientInfo clientInfo)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            var storedToken = await _context.RefreshTokens
                .Where(x => x.Token == request.RefreshToken)
                .FirstOrDefaultAsync();

            if (storedToken == null)
                return null;

            if (!storedToken.IsActive)
                return null;

            var user = await _userManager.FindByIdAsync(storedToken.UserId);
            if (user == null)
                return null;

            // revoke old refresh token
            storedToken.IsRevoked = true;
            storedToken.RevokedAt = DateTime.UtcNow;
            storedToken.RevokedByIp = clientInfo.Ip;
            storedToken.RevokedByUserAgent = clientInfo.UserAgent;

            // generate new tokens
            var newToken = await _tokenService.CreateTokenAsync(user);

            var refreshTokenDays = storedToken.RememberMe
                ? _config.GetValue<int>("Jwt:RememberMeRefreshTokenDays")
                : _config.GetValue<int>("Jwt:RefreshTokenDays");


            var newRefreshEntity = new RefreshToken
            {
                Token = newToken.RefreshToken,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenDays),
                CreatedByIp = clientInfo.Ip,
                CreatedByUserAgent = clientInfo.UserAgent,
                RememberMe = storedToken.RememberMe
            };

            storedToken.ReplacedByToken = newRefreshEntity.Token;


            _context.RefreshTokens.Add(newRefreshEntity);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return newToken;
        }


        public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordRequest request) // Login olmuş kullanıcı
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.ChangePasswordAsync(
                user,
                request.CurrentPassword,
                request.NewPassword
            );

            return result.Succeeded;
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return true; // security: email var mı yok mu belli etme

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // TODO: burada mail gönderilecek
            // örnek: https://yourapp.com/reset-password?email=...&token=...

            return true;
        }
        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return false;

            var result = await _userManager.ResetPasswordAsync(
                user,
                request.Token,
                request.NewPassword
            );

            return result.Succeeded;
        }


    }
}
