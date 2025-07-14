global using FastEndpoints;
using FastEndpoints.Swagger;
using ThreadPilot.Insurance.Api.ApiClients;
using ThreadPilot.Insurance.Api.Initialization;
using ThreadPilot.Insurance.Api.Repository;

var builder = WebApplication.CreateBuilder();

builder.Services.AddSingleton<IVehicleApiClient, VehicleApiClient>();
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
    .AddScoped<IReadInsuranceRepository, ReadInsuranceRepository>()
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