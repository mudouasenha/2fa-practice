using Doodle.Infrastructure.Repository.Data.Contexts;
using Doodle.Infrastructure.Repository.Data.Seeds;
using Doodle.Infrastructure.Repository.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace Doodle.Infrastructure.Repository.Extensions
{
    public static class HostBuilderExtensions
    {
        public static async Task InitializeAndRunAsync(this IHost host)
        {
            host.RunMigrations<DoodleDbContext>();
            await host.RunAsync();
        }

        public static IHost RunMigrations<TContext>(this IHost host) where TContext : IdentityDbContext
        {
            try
            {
                using var scope = host.Services.CreateScope();
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<TContext>();
                var seedOptions = services.GetRequiredService<IOptions<SeedOptions>>();
                UserIdentitySeed.Seed(dbContext, seedOptions.Value.Password);
                dbContext.Database.Migrate();

                return host;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Message={Message}; Method={Method}",
                    ex.Message,
                    nameof(RunMigrations));
                throw;
            }
        }
    }
}