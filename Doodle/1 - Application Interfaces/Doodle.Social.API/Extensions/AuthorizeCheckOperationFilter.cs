using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Data;

namespace Doodle.Api.Extensions
{
    public class AuthorizeCheckOperationFilter : IOperationFilter, IDocumentFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizeCheckOperationFilter(IHttpContextAccessor httpContextAccessor) =>
            _httpContextAccessor = httpContextAccessor;

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAuthorize =
              context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
              || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (hasAuthorize)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "oauth2"
                                }
                            }
                        ] = new[] {"api1"}
                    }
                };
            }
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var user = _httpContextAccessor.HttpContext.User;
            var isAuthtenticated = user?.Identity?.IsAuthenticated ?? false;

            List<string> pathsToRemove = new();
            if (!isAuthtenticated)
            {
                foreach (var item in context.ApiDescriptions)
                {
                    var isAllowAnonymous = item.CustomAttributes().Any(_ => _ is AllowAnonymousAttribute);

                    if (!isAllowAnonymous)
                        pathsToRemove.Add("/" + item.RelativePath);
                }
                return;
            }

            List<string> pathToRemove = new();
            foreach (var item in context.ApiDescriptions)
            {
                var rolesFromAttribute = item.CustomAttributes()
                    .OfType<AuthorizeAttribute>()
                    .Select(a => a.Roles)
                    .Distinct();

                if (rolesFromAttribute.Any())
                {
                    string roleAttribute = rolesFromAttribute.FirstOrDefault();
                    if (roleAttribute != null)
                    {
                        string[] roles = roleAttribute.Split(',');
                        // if roles contains the given role then it mean i have to keep this path
                        if (!roles.Any(role => user.IsInRole(role)))
                        {
                            pathToRemove.Add("/" + item.RelativePath);
                        }
                    }
                }
            }

            pathsToRemove.ForEach(x => { swaggerDoc.Paths.Remove(x); });
        }
    }
}