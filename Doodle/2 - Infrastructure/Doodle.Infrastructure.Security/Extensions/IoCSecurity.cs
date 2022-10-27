using Doodle.Infrastructure.Security.Models.Options;
using Doodle.Infrastructure.Security.MultiFactorAuthentication;
using Doodle.Infrastructure.Security.MultiFactorAuthentication.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Doodle.Infrastructure.Security.Extensions
{
    public static class IoCSecurity
    {
        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<OpenIdConnectKeyOptions>(opt => config.GetSection(nameof(OpenIdConnectKeyOptions)).Bind(opt));
            services.Configure<TwilioOptions>(opt => config.GetSection(nameof(TwilioOptions)).Bind(opt));

            services.AddCors();

            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddUserManager<ApplicationIdentityUserManager>()
                .AddDefaultUI()
                .AddEntityFrameworkStores<DoodleAuthDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, AdditionalUserClaimsPrincipalFactory>();
            services.AddScoped<IVerification, Twilio2FAVerifyService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("TwoFactorEnabled", x => x.RequireClaim("amr", "mfa"));
                options.AddPolicy("AdminOnly", x => x.RequireClaim("Role", "Admin"));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.AccessDeniedPath = "/Areas/Identity/Pages/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromSeconds(30);
            })
            .AddOpenIdConnect(options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority = config["OpenIdConnectKeyOptions:Authority"];
                options.RequireHttpsMetadata = false;
                options.ClientId = config["OpenIdConnectKeyOptions:ClientId"];
                options.ClientSecret = config["OpenIdConnectKeyOptions:ClientSecret"];
                // Code with PKCE can also be used here
                options.ResponseType = "code id_token";
                options.Scope.Add("profile");
                options.Scope.Add("offline_access");
                options.SaveTokens = true;
                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProvider = context =>
                    {
                        context.ProtocolMessage.SetParameter("acr_values", "mfa");
                        return Task.FromResult(0);
                    }
                };
            });

            services.ConfigureSecurity();

            return services;
        }

        public static IServiceCollection ConfigureSecurity(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = true;
            });

            return services;
        }
    }
}