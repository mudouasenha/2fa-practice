using Doodle.Api.Extensions;
using Doodle.Infrastructure.Repository.Extensions;
using Doodle.Infrastructure.Security.Extensions;
using Doodle.Services.Extensions;
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
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddRepositoryInfrastructure(builder.Configuration)
    .AddSecurity(builder.Configuration)
    .AddServices()

    .Configure<RouteOptions>(options => options.LowercaseUrls = true);

var app = builder.Build();

// Configuration
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
        .UseSwaggerUI();
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