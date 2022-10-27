namespace Doodle.Presentation.Extensions;

public static class IoCServices
{
    public static IMvcBuilder AddPresentation(this IServiceCollection services) =>
        services.AddRazorPages();

    public static void UsePresentation(this WebApplication builder) =>
        builder.MapRazorPages();
}