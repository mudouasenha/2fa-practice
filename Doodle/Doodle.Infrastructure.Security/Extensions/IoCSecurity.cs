using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Doodle.Infrastructure.Security.Extensions
{
    public static class IoCSecurity
    {
        public static IServiceCollection AddSecurity(this IServiceCollection services)
        {
            /*services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<DoodleDbContext>().AddDefaultTokenProviders();*/

            services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, AdditionalUserClaimsPrincipalFactory>()
            .AddAuthorization(options => options.AddPolicy("TwoFactorEnabled", x => x.RequireClaim("amr", "mfa")));

            services.AddAuthorization(options => options.AddPolicy("TwoFactorEnabled", x => x.RequireClaim("amr", "mfa")));

            return services;
        }
    }
}