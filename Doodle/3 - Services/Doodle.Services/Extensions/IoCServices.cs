using Doodle.Services.Extensions.Policies;
using Doodle.Services.Users;
using Doodle.Services.Users.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Doodle.Services.Extensions;

public static class IoCServices
{
    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services.AddScoped<IUsersService, UsersService>();

    public static IServiceCollection AddExampleHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient("Example").AddPolicyHandler(request => HttpClientPolicies.ExponentialBackOffHttpRetry);

        return services;
    }
}