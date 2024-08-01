using DesafioTecnico.Service.DotNet8.Api.Configurations;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using DesafioTecnico.Service.DotNet8.InfrastructureCore.InversionOfControl;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration(); // Usar a configura��o do Swagger
builder.Services.AddDependencyResolver();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration(); // Usar a configura��o do Swagger
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Starting up the application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}

[ExcludeFromCodeCoverage]
public partial class Program
{ }
