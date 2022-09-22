using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doodle.Infrastructure.Repository.Extensions
{
    public static class HostBuilderExtensions
    {
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
