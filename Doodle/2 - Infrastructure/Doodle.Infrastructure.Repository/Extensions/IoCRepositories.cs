using Doodle.Infrastructure.Repository.Data;
using Doodle.Infrastructure.Repository.Data.Contexts;
using Doodle.Infrastructure.Repository.Options;
using Doodle.Infrastructure.Repository.Repositories;
using Doodle.Infrastructure.Repository.Repositories.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Doodle.Infrastructure.Repository.Extensions
{
    public static class IoCRepositories
    {
        public static IServiceCollection AddRepositoryInfrastructure(this IServiceCollection services, IConfiguration config) =>
            services.BindOptions(config)
                    .AddDbContext<DoodleDbContext>(options => options.UseSqlServer(config.GetConnectionString("Doodle")))
                        .AddAsyncInitializer<DbContextInitializer<DoodleDbContext>>()
                    .AddRepositories()
                    .AddDatabaseTransaction()
                    .AddDatabaseDeveloperPageExceptionFilter();

        public static IServiceCollection AddRepositories(this IServiceCollection services) =>
            services.AddScoped<IUserRepository, UserRepository>();

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