using Doodle.Infrastructure.Repository.Data;
using Doodle.Infrastructure.Repository.Repositories;
using Doodle.Infrastructure.Repository.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Doodle.Infrastructure.Repository.Extensions;

namespace Doodle.Infrastructure.Repository.Extensions
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
