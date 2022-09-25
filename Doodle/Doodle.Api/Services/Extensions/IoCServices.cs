using Doodle.Api.Services.Users;
using Doodle.Api.Services.Users.Abstractions;

namespace Doodle.Api.Services.Extensions;

public static class IoCServices
{
    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services.AddScoped<IUsersService, UsersService>();
}