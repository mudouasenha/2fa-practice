using Doodle.Domain.Constants;
using Doodle.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Doodle.Auth.Infrastructure.Repository.Data.Seeds
{
    public static class UserIdentitySeed
    {
        public static ModelBuilder SeedApplicationUsers(this ModelBuilder builder, string password)
        {
            CreateUserAsync(builder, 1, "Administrator", "admin", "admin@admin.com", password, new List<(int, string)> { (1, RoleConstants.Admin), (2, RoleConstants.Reader) });
            CreateUserAsync(builder, 2, "Reader", "reader", "reader@reader.com", password, new List<(int, string)> { (2, RoleConstants.Reader) });

            return builder;
        }

        public static ModelBuilder CreateUserAsync(this ModelBuilder builder, int id, string name, string userName, string email, string password, List<(int roleId, string roleName)> roles)
        {
            var user = new ApplicationUser()
            {
                Name = name,
                UserName = userName,
                PhoneNumber = "+5599999999999",
                NormalizedUserName = userName,
                Email = email,
                NormalizedEmail = email,
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                Id = id,
                CreatedAt = DateTime.Now
            };

            PasswordHasher<ApplicationUser> hasher = new();

            user.PasswordHash = hasher.HashPassword(user, password);

            builder.Entity<ApplicationUser>().HasData(user);

            foreach (var role in roles)
            {
                builder.Entity<IdentityUserRole<int>>().HasData(
                    new IdentityUserRole<int> { RoleId = role.roleId, UserId = user.Id }
                );
            }

            return builder;
        }
    }
}