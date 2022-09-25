using Doodle.Api.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Doodle.Api.Repository.Extensions
{
    public static class HostBuilderExtensions
    {
        public static async Task InitializeAndRunAsync(this IHost host)
        {
            host.RunMigrations<DoodleDbContext>();
            await host.RunAsync();
        }

        public static IHost RunMigrations<TContext>(this IHost host) where TContext : DbContext
        {
            try
            {
                using var scope = host.Services.CreateScope();
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<TContext>();
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