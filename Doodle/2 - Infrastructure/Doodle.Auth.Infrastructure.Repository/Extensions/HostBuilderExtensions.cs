using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Doodle.Auth.Infrastructure.Repository.Data.Contexts;
using Doodle.Auth.Infrastructure.Repository.Data.Seeds;
using Doodle.Auth.Infrastructure.Repository.Extensions;
using Doodle.Auth.Infrastructure.Repository.Options;

namespace Doodle.Auth.Infrastructure.Repository.Extensions
{
    public static class HostBuilderExtensions
    {
        public static async Task InitializeAndRunAsync(this IHost host)
        {
            host.RunMigrations<DoodleAuthDbContext>();
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
