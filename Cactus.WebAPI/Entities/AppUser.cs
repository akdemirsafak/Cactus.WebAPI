using Microsoft.AspNetCore.Identity;

namespace Cactus.WebAPI.Entities
{
    public class AppUser : IdentityUser<string>
    {
        public string? FullName { get; set; }
    }
}
