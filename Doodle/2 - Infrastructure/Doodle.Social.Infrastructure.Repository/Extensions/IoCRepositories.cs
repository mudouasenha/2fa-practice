using Doodle.Social.Infrastructure.Repository.Data.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Doodle.Social.Infrastructure.Repository.Extensions
{
    public static class IoCRepositories
    {
        public static IServiceCollection AddRepositoryInfrastructure(this IServiceCollection services, IConfiguration config) =>
            services.BindOptions(config)
                    .AddDbContext<DoodleSocialDbContext>(options => options.UseSqlServer(config.GetConnectionString("Doodle")))
                        .AddAsyncInitializer<DbContextInitializer<DoodleSocialDbContext>>()
                    .AddRepositories()
                    .AddDatabaseTransaction()
                    .AddDatabaseDeveloperPageExceptionFilter();

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        { return services; }

        public static IServiceCollection AddDatabaseTransaction(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection BindOptions(this IServiceCollection services, IConfiguration config)
        {
            return services;
        }

        public static IApplicationBuilder UseDatabaseExceptionFilter(this IApplicationBuilder builder) => builder.UseMigrationsEndPoint();
    }
}