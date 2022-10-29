using Microsoft.AspNetCore.Identity;

namespace Doodle.Domain.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public bool MfaEnabled { get; set; }

        public bool Verified { get; set; }

        public string MfaIdentity { get; set; }

        public string Salt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}