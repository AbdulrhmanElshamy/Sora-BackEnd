using Microsoft.AspNetCore.Identity;
using Sofra.Api.Enums;

namespace Sofra.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; } = [];
    }
}
