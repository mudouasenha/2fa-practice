using Doodle.Services.Extensions.Policies;
using Doodle.Services.Options;
using Doodle.Services.Security;
using Doodle.Services.Security.Abstractions;
using Doodle.Services.Users;
using Doodle.Services.Users.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Doodle.Services.Extensions;

public static class IoCServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<EmailSenderOptions>(opt => config.GetSection(nameof(EmailSenderOptions)).Bind(opt));

        services.AddScoped<IUsersService, UsersService>()
            .AddScoped<ICryptoService, CryptoService>();

        return services;
    }

    public static IServiceCollection AddExampleHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient("Example").AddPolicyHandler(request => HttpClientPolicies.ExponentialBackOffHttpRetry);

        return services;
    }
}