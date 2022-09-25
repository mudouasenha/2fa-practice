using Doodle.Api.Repository.Repositories;
using Doodle.Api.Repository.Repositories.Abstractions;
using Doodle.Infrastructure.Repository.Data;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Doodle.Api.Repository.Extensions
{
    public static class IoCRepositories
    {
        public static IServiceCollection AddRepositoryInfrastructure(this IServiceCollection services) =>
            services.AddRepositories()
                    .AddDatabaseTransaction();

        public static IServiceCollection AddRepositories(this IServiceCollection services) =>
            services.AddScoped<IUserRepository, UserRepository>();

        public static IServiceCollection AddDatabaseTransaction(this IServiceCollection services)
        {
            services.TryAddScoped<IDatabaseTransaction, DoodleDatabaseTransaction>();

            return services;
        }
    }
}