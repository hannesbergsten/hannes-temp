using System.Collections.Immutable;
using ThreadPilot.Insurance.Api.ApiClients;
using ThreadPilot.Insurance.Api.Extensions;
using ThreadPilot.Insurance.Api.Models;
using ThreadPilot.Insurance.Api.Repository;
using ThreadPilot.Shared.Models;

namespace ThreadPilot.Insurance.Api.Services;

public interface IInsuranceService
{
    Task<InsuranceResponse> ProcessInsurances(string personalId, CancellationToken ct);
}

public class InsuranceService(IReadInsuranceRepository insuranceRepository, IVehicleApiClient vehicleApiClient)
    : IInsuranceService
{
    public async Task<InsuranceResponse> ProcessInsurances(string personalId, CancellationToken ct)
    {
        var insurancesFromDb = await insuranceRepository
            .GetInsurancesByPersonalId(personalId, ct)
            .ConfigureAwait(false);

        if (insurancesFromDb.Count == 0) return InsuranceResponse.Empty;

        var insurances = await EnrichInsuranceTypes(insurancesFromDb, ct).ConfigureAwait(false);

        return insurances.ToResponse();
    }

    private async Task<ImmutableArray<InsuranceBase>> EnrichInsuranceTypes(IReadOnlyList<InsuranceBase> insurances, CancellationToken ct)
    {
        var vehicleInsurances = GetVehicleInsurances(insurances);
        if (vehicleInsurances.Count == 0) return [..insurances];

        var registrationNumbers = GetRegistrationNumbers(vehicleInsurances);
        if (registrationNumbers.Count == 0) return [..insurances];
        
        var vehicleResponse = await vehicleApiClient.GetVehiclesByRegistrationNumber(registrationNumbers, ct)
            .ConfigureAwait(false);
      
        return [
            ..insurances.Select(insurance => insurance switch
            {
                VehicleInsurance { Vehicle.RegistrationNumber: not null } vehicleInsurance =>
                    UpdateVehicleInsurance(vehicleInsurance, vehicleResponse),
                _ => insurance
            })
        ];
    }

    private static InsuranceBase UpdateVehicleInsurance(VehicleInsurance insurance, IReadOnlyList<Vehicle> vehicles)
    {
        var vehicle = vehicles.FirstOrDefault(v => 
            v.RegistrationNumber == insurance.Vehicle!.RegistrationNumber);
        return vehicle != null ? insurance with { Vehicle = vehicle } : insurance;
    }

    private static IReadOnlyList<string> GetRegistrationNumbers(IReadOnlyList<VehicleInsurance> vehicleInsurances)
    {
        return vehicleInsurances
            .Select(v => v.Vehicle!.RegistrationNumber!)
            .ToList();
    }

    private static IReadOnlyList<VehicleInsurance> GetVehicleInsurances(IReadOnlyList<InsuranceBase> insurances)
    {
        return insurances.OfType<VehicleInsurance>()
            .Where(v => v.Vehicle?.RegistrationNumber != null)
            .ToList();
    }
}