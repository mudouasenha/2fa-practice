using Doodle.Api.Extensions;
using Doodle.Infrastructure.Repository.Extensions;
using Doodle.Infrastructure.Security.Extensions;
using Doodle.Services.Extensions;
using Hellang.Middleware.ProblemDetails;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    EnvironmentName = Environments.Development,
});

SerilogExtensions.AddSerilogApi(builder.Configuration);
builder.Host.UseSerilog(Log.Logger);

// Add services to the container.
builder.Services.AddProblemDetails(setup => setup.IncludeExceptionDetails = (ctx, env) =>
Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ||
Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Staging");

builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Protected API", Version = "v1" });

    /*options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://localhost:7077/connect/authorize"),
                TokenUrl = new Uri("https://localhost:7077/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    {"api1", "Demo API - full access"}
                }
            }
        }
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="oauth2"
                }
            },
            new string[]{}
        }
    });

    options.DocumentFilter<AuthorizeCheckOperationFilter>();
    options.OperationFilter<AuthorizeCheckOperationFilter>();*/
});

builder.Services.AddRepositoryInfrastructure(builder.Configuration)
    .AddSecurity(builder.Configuration)
    .AddServices(builder.Configuration)
    .Configure<RouteOptions>(options => options.LowercaseUrls = true);

var app = builder.Build();

// Configuration
app.UseProblemDetails();
app.UseRouting();

app.UseCors(x =>
{
    x.AllowAnyOrigin();
    x.AllowAnyHeader();
    x.AllowAnyHeader();
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage()
        .UseDatabaseExceptionFilter()
        .UseSwagger()
        .UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger", "Doodle API V1");

            /*options.OAuthClientId("demo_api_swagger");
            options.OAuthAppName("Demo API - Swagger");
            options.OAuthUsePkce();*/
        });
}
else
{
    app.UseExceptionHandler("/Error")
        .UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapRazorPages();

await app.InitializeAndRunAsync();