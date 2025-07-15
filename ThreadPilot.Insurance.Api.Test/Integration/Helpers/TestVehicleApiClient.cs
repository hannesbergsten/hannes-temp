using ThreadPilot.Insurance.Api.ApiClients;
using ThreadPilot.Shared.Models;

namespace ThreadPilot.Insurance.Api.Test.Integration.Helpers;

public class TestVehicleApiClient : IVehicleApiClient
{
    public Task<IReadOnlyList<Vehicle>> GetVehiclesByRegistrationNumber(IReadOnlyList<string> registrationIds, CancellationToken ct)
    {
        var vehicles = new List<Vehicle>
        {
            new() { RegistrationNumber = "REGNR1", Manufacturer = "Volvo", Year = 2020 },
            new() { RegistrationNumber = "REGNR2", Manufacturer = "BMW", Year = 2018 },
            new() { RegistrationNumber = "REGNR3", Manufacturer = "Opel", Year = 2022 }
        };

        var result = vehicles.Where(v => registrationIds.Contains(v.RegistrationNumber));
        return Task.FromResult<IReadOnlyList<Vehicle>>(result.ToList());
    }
}