using Doodle.Api.Extensions;
using Doodle.Infrastructure.Repository.Data.Contexts;
using Doodle.Infrastructure.Repository.Extensions;
using Doodle.Infrastructure.Security.Extensions;
using Doodle.Services.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    EnvironmentName = Environments.Development,
});

SerilogExtensions.AddSerilogApi(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer()
    .AddDbContext<DoodleDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Doodle")))
        .AddAsyncInitializer<DbContextInitializer<DoodleDbContext>>()
    .AddSwaggerGen()
    .AddRepositoryInfrastructure()
    .AddServices()
    .AddCors()
    .Configure<RouteOptions>(options => options.LowercaseUrls = true)
    .AddSecurity()
    .AddRazorPages();

var app = builder.Build();

app.UseRouting();

app.UseCors(x =>
{
    x.AllowAnyOrigin();
    x.AllowAnyHeader();
    x.AllowAnyHeader();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

await app.InitializeAndRunAsync();