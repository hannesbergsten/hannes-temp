using ThreadPilot.Insurance.Api.ApiClients;
using ThreadPilot.Insurance.Api.Extensions;
using ThreadPilot.Insurance.Api.Models;
using ThreadPilot.Insurance.Api.Repository;

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

    private async Task<InsuranceBase[]> EnrichInsuranceTypes(List<InsuranceBase> insurances, CancellationToken ct)
    {
        var vehicleInsurances = GetVehicleInsurances(insurances);
        if (vehicleInsurances.Count == 0) return insurances.ToArray();

        var registrationNumbers = GetRegistrationNumbers(vehicleInsurances);
        if (registrationNumbers.Count == 0) return insurances.ToArray();
        
        var vehicleResponse = await vehicleApiClient.GetVehiclesByRegistrationNumber(registrationNumbers, ct)
            .ConfigureAwait(false);

        foreach (var insurance in vehicleInsurances)
        {
            var vehicle = vehicleResponse.FirstOrDefault(v =>
                v.RegistrationNumber == insurance.Vehicle!.RegistrationNumber);

            if (vehicle != null)
            {
                var index = insurances.IndexOf(insurance);
                insurances[index] = insurance with { Vehicle = vehicle };
            }
        }

        return insurances.ToArray();
    }

    private static List<string> GetRegistrationNumbers(List<VehicleInsurance> vehicleInsurances)
    {
        return vehicleInsurances
            .Select(v => v.Vehicle!.RegistrationNumber!)
            .ToList();
    }

    private static List<VehicleInsurance> GetVehicleInsurances(List<InsuranceBase> insurances)
    {
        return insurances.OfType<VehicleInsurance>()
            .Where(v => v.Vehicle?.RegistrationNumber != null)
            .ToList();
    }
}