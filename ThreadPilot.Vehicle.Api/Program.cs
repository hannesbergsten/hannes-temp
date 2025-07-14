global using FastEndpoints;
using FastEndpoints.Swagger;
using ThreadPilot.Vehicle.Api.Initialization;
using ThreadPilot.Vehicle.Api.Repository;

var builder = WebApplication.CreateBuilder();

builder.Services
    .AddFastEndpoints()
    .AddScoped<IVehicleRepository, VehicleRepository>()
    .AddAppSwagger();

var app = builder.Build();

app.UseDefaultExceptionHandler()
    .UseFastEndpoints(c =>
    {
        // add /api/v1/...
        c.Versioning.Prefix = "v";
        c.Versioning.DefaultVersion = 1; 
        c.Versioning.PrependToRoute = true;
        c.Endpoints.RoutePrefix = "api";
    })
    .UseSwaggerGen();

app.Run();

public partial class Program;