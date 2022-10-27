using Doodle.Services.Auth.Security.Abstractions;
using Doodle.Services.Auth.Users;
using Doodle.Services.Auth.Users.Abstractions;
using Doodle.Services.EmailSender;
using Doodle.Services.EmailSender.Abstractions;
using Doodle.Services.Extensions.Policies;
using Doodle.Services.Options;
using Doodle.Services.Security;
using Doodle.Services.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Doodle.Services.Extensions;

public static class IoCServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<EmailSenderOptions>(opt => config.GetSection(nameof(EmailSenderOptions)).Bind(opt));

        services.AddScoped<IUserRegistrationService, UserRegistrationService>()
            .AddScoped<IUserSessionService, UserSessionService>()
            .AddScoped<IEmailSenderService, EmailSenderService>()
            .AddScoped<ITokenService, TokenService>();

        return services;
    }

    public static IServiceCollection AddExampleHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient("Example").AddPolicyHandler(request => HttpClientPolicies.ExponentialBackOffHttpRetry);

        return services;
    }
}