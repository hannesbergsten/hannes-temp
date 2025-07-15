namespace ThreadPilot.Vehicle.Api.Repository;
using Shared.Models;
public interface IVehicleRepository
{
    Task<List<Vehicle>> GetVehiclesByRegistrationNumber(List<string> registrationNumbers, CancellationToken ct);
}

public class VehicleRepository : IVehicleRepository
{
    public async Task<List<Vehicle>> GetVehiclesByRegistrationNumber(List<string> registrationNumbers, CancellationToken ct)
    {
        var dbVehicles = new List<Vehicle>
        {
            new() { RegistrationNumber = "REGNR1", Manufacturer = "Volvo", Year = 2020 },
            new() { RegistrationNumber = "REGNR2", Manufacturer = "BMW", Year = 2018 },
            new() { RegistrationNumber = "REGNR3", Manufacturer = "Opel", Year = 2022 }
        };

        var result = dbVehicles.Where(v => registrationNumbers.Contains(v.RegistrationNumber)).ToList();
        return await Task.FromResult(result);
    }
}