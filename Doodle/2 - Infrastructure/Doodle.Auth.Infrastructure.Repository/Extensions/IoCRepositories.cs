using Doodle.Auth.Infrastructure.Repository.Data;
using Doodle.Auth.Infrastructure.Repository.Data.Contexts;
using Doodle.Auth.Infrastructure.Repository.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Doodle.Auth.Infrastructure.Repository.Extensions
{
    public static class IoCRepositories
    {
        public static IServiceCollection AddRepositoryInfrastructure(this IServiceCollection services, IConfiguration config) =>
            services.BindOptions(config)
                    .AddDbContext<DoodleAuthDbContext>(options => options.UseSqlServer(config.GetConnectionString("Doodle")))
                        .AddAsyncInitializer<DbContextInitializer<DoodleAuthDbContext>>()
                    .AddRepositories()
                    .AddDatabaseTransaction()
                    .AddDatabaseDeveloperPageExceptionFilter();

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        { return services; }

        public static IServiceCollection AddDatabaseTransaction(this IServiceCollection services)
        {
            services.TryAddScoped<IDatabaseTransaction, DoodleDatabaseTransaction>();

            return services;
        }

        public static IServiceCollection BindOptions(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<SeedOptions>(opt => config.GetSection(nameof(SeedOptions)).Bind(opt));

            return services;
        }

        public static IApplicationBuilder UseDatabaseExceptionFilter(this IApplicationBuilder builder) => builder.UseMigrationsEndPoint();
    }
}