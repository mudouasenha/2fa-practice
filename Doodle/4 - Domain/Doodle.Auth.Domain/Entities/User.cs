using Microsoft.AspNet.Identity.EntityFramework;

namespace Doodle.Auth.Domain.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        public string Username { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public bool MfaEnabled { get; set; }

        public bool Verified { get; set; }

        public string MfaIdentity { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public string PhoneNumber { get; set; }
    }
}