using Doodle.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Doodle.Auth.Infrastructure.Repository.Data.Seeds
{
    public static class RolesSeed
    {
        public static void SeedRoles(this ModelBuilder builder)
        {
            builder.Entity<IdentityRole<int>>().HasData(new List<IdentityRole<int>>
            {
                new IdentityRole<int> { Id = 1, Name = RoleConstants.Admin, NormalizedName = RoleConstants.Admin.ToUpper() },
                new IdentityRole<int> { Id = 2, Name = RoleConstants.Reader, NormalizedName = RoleConstants.Reader.ToUpper() }
            });
        }
    }
}