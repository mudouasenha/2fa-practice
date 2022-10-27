using Doodle.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Doodle.Infrastructure.Repository.Auth.Data.Seeds
{
    public class UserIdentitySeed
    {
        public static void Seed<TContext>(TContext context, string password) where TContext : IdentityDbContext
        {
            context.Database.EnsureCreated();
            CreateUserAsync(context, "admin@admin.com", password, RoleConstants.Admin).GetAwaiter().GetResult();
            CreateUserAsync(context, "reader@reader.com", password, RoleConstants.Reader).GetAwaiter().GetResult();
            context.SaveChanges();
        }

        public static async Task CreateUserAsync<TContext>(TContext context, string email, string password, string role) where TContext : IdentityDbContext
        {
            var user = new IdentityUser
            {
                UserName = email,
                NormalizedUserName = email,
                Email = email,
                NormalizedEmail = email,
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var roleStore = new RoleStore<IdentityRole>(context);

            if (!context.Roles.Any(r => r.Name == role))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = role, NormalizedName = role });
            }

            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var passwordHasher = new PasswordHasher<IdentityUser>();
                var hashed = passwordHasher.HashPassword(user, password);
                user.PasswordHash = hashed;
                var userStore = new UserStore<IdentityUser>(context);
                await userStore.CreateAsync(user);
                await userStore.AddToRoleAsync(user, role);
            }
        }
    }
}
