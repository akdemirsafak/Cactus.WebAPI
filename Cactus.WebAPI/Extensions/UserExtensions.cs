using System.Security.Claims;

namespace Cactus.WebAPI.Extensions
{
    public static class UserExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("UserId claim bulunamadı.");

            return Guid.Parse(userId);
        }
    }
}
