using Doodle.Auth.Infrastructure.Repository.Data.Seeds;
using Doodle.Auth.Infrastructure.Repository.Options;
using Doodle.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Doodle.Auth.Infrastructure.Repository.Data.Contexts
{
    public class DoodleAuthDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        private readonly IOptions<SeedOptions> _options;

        public DoodleAuthDbContext(DbContextOptions<DoodleAuthDbContext> options, IOptions<SeedOptions> seedOptions) : base(options)
        {
            _options = seedOptions;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.SeedRoles();
            modelBuilder.SeedApplicationUsers(_options.Value.Password);

            base.OnModelCreating(modelBuilder);
        }
    }
}