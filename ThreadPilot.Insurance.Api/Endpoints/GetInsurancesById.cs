using System.Net;
using ThreadPilot.Insurance.Api.ApiClients;
using ThreadPilot.Insurance.Api.Models;
using ThreadPilot.Insurance.Api.Repository;
using ThreadPilot.Insurance.Api.Extensions;
using ThreadPilot.Insurance.Api.Models.Requests;

namespace ThreadPilot.Insurance.Api.Endpoints;

public class GetInsurancesById(IReadInsuranceRepository insuranceRepository, IVehicleApiClient vehicleApiClient)
    : Endpoint<GetInsurancesByIdRequest, InsuranceResponse>
{
    public override void Configure()
    {
        Get("/insurances/{id}");
        Description(x => x.WithName("GetInsurancesById"));
        AllowAnonymous();
        Validator<GetInsurancesByIdRequestValidator>();
    }

    public override async Task HandleAsync(GetInsurancesByIdRequest req, CancellationToken ct)
    {
        //TODO move to an insurance service
        var insurances = await insuranceRepository
            .GetInsurancesByPersonalId(req.Id, ct)
            .ConfigureAwait(false);

        if (insurances.Length == 0)
        {
            await SendNotFoundAsync(ct).ConfigureAwait(false);
            return;
        }

        var mappedInsurances = await GetMappedInsurances(insurances.ToList(), ct).ConfigureAwait(false);
        if (mappedInsurances.Length == 0)
        {
            await SendNotFoundAsync(ct).ConfigureAwait(false);
            return;
        }

        await SendOkAsync(mappedInsurances.ToResponse(), ct)
            .ConfigureAwait(false);
    }

    private async Task<InsuranceBase[]> GetMappedInsurances(List<InsuranceBase> insurances, CancellationToken ct)
    {
        var vehicleInsuranceTasks = new List<Task>();

        MapInsurancesByType(insurances, ct, vehicleInsuranceTasks);
        
        await Task.WhenAll(vehicleInsuranceTasks);
        
        return insurances.ToArray();
    }

    private void MapInsurancesByType(List<InsuranceBase> insurances, CancellationToken ct, List<Task> vehicleInsuranceTasks)
    {
        for (var i = 0; i < insurances.Count; i++)
        {
            switch (insurances[i])
            {
                case VehicleInsurance { Vehicle.RegistrationNumber: not null } vehicleInsurance:
                    vehicleInsuranceTasks.Add(AddVehicleInfo(insurances, ct, vehicleInsurance, i));
                    break;

                case PetInsurance:
                case HealthInsurance:
                    break;
            }
        }
    }

    private async Task AddVehicleInfo(List<InsuranceBase> insurances, CancellationToken ct,
        VehicleInsurance vehicleInsurance,
        int i)
    {
        var vehicle = await vehicleApiClient
            .GetVehicleByRegistrationNumber(vehicleInsurance.Vehicle!.RegistrationNumber!, ct)
            .ConfigureAwait(false);

        if (vehicle is not null)
        {
            insurances[i] = vehicleInsurance with { Vehicle = vehicle } as VehicleInsurance;
        }
    }
}