using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Doodle.Infrastructure.Repository.Auth.Data.Contexts
{
    public class DoodleAuthDbContext : IdentityDbContext
    {
        public DoodleAuthDbContext(DbContextOptions<DoodleAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}