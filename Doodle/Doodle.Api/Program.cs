using Doodle.Api.Extensions;
using Doodle.Infrastructure.Repository.Data.Contexts;
using Doodle.Infrastructure.Repository.Extensions;
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
    .AddSwaggerGen()
    .AddDbContext<DoodleDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Doodle")))
    .AddRepositoryInfrastructure()
    .AddServices(builder.Configuration)
    .AddCors();

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

app.Run();