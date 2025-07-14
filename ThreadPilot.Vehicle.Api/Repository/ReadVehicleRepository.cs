namespace ThreadPilot.Vehicle.Api.Repository;
using Shared.Models;
public interface IVehicleRepository
{
    Task<Vehicle?> GetVehicleByRegistrationNumber(string registrationNumber, CancellationToken ct);
}

public class VehicleRepository : IVehicleRepository
{
    public async Task<Vehicle?> GetVehicleByRegistrationNumber(string registrationNumber, CancellationToken ct)
    {
        var vehicles = new List<Vehicle>
        {
            new() { RegistrationNumber = "REGNR1", Manufacturer = "Volvo", Year = 2020 },
            new() { RegistrationNumber = "REGNR2", Manufacturer = "BMW", Year = 2018 },
            new() { RegistrationNumber = "REGNR3", Manufacturer = "Opel", Year = 2022 }
        };

        var vehicle = vehicles.FirstOrDefault(v => v.RegistrationNumber == registrationNumber);
        return await Task.FromResult(vehicle);
    }
}