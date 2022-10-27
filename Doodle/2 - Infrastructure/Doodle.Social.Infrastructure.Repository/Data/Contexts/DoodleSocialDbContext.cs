using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Doodle.Social.Infrastructure.Repository.Data.Contexts
{
    public class DoodleSocialDbContext : DbContext
    {
        public DoodleSocialDbContext(DbContextOptions<DoodleSocialDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}