using Doodle.Infrastructure.Repository.Data.Contexts;
using Doodle.Infrastructure.Security.Models.Options;
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
            services.AddCors();

            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddDefaultUI()
                .AddEntityFrameworkStores<DoodleDbContext>().AddDefaultTokenProviders();

            services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, AdditionalUserClaimsPrincipalFactory>()
                .AddAuthorization(options => options.AddPolicy("TwoFactorEnabled", x => x.RequireClaim("amr", "mfa")));

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie()
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

            return services;
        }
    }
}