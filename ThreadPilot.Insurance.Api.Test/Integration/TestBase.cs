using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ThreadPilot.Insurance.Api.ApiClients;
using ThreadPilot.Insurance.Api.Services;
using ThreadPilot.Insurance.Api.Test.Integration.Helpers;

namespace ThreadPilot.Insurance.Api.Test.Integration;

public class TestBase
{
    protected HttpClient CreateClientFromFactory()
    {
        var customFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddScoped<IVehicleApiClient, TestVehicleApiClient>();
                services.AddScoped<IInsuranceService, InsuranceService>();
            });
        });

        return customFactory.CreateClient();

    }
}