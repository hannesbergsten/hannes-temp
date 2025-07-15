global using FastEndpoints;
using FastEndpoints.Swagger;
using ThreadPilot.Insurance.Api.ApiClients;
using ThreadPilot.Insurance.Api.Initialization;
using ThreadPilot.Insurance.Api.Repository;
using ThreadPilot.Insurance.Api.Services;

var builder = WebApplication.CreateBuilder();

builder.Services
    .AddScoped<IReadInsuranceRepository, ReadInsuranceRepository>()
    .AddSingleton<IVehicleApiClient, VehicleApiClient>()
    .AddScoped<IInsuranceService, InsuranceService>();

builder.Services.AddHttpClient<IVehicleApiClient, VehicleApiClient>((provider, options) =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();

    var url = configuration.GetValue<string>("VehicleApiClient:BaseUrl");
    ArgumentException.ThrowIfNullOrEmpty(url, nameof(url));
    options.BaseAddress = new Uri(url!);
    options.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services
    .AddFastEndpoints()
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