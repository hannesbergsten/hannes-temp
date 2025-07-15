using ThreadPilot.Insurance.Api.ApiClients;
using ThreadPilot.Insurance.Api.Models;
using ThreadPilot.Insurance.Api.Models.Requests;
using ThreadPilot.Insurance.Api.Services;

namespace ThreadPilot.Insurance.Api.Endpoints;

public class GetInsurancesById(IInsuranceService insuranceService, IVehicleApiClient vehicleApiClient)
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
        var results = await insuranceService.ProcessInsurances(req.Id, ct).ConfigureAwait(false);

        if (results.Insurances.Length != 0)
        {
            await SendOkAsync(results, ct).ConfigureAwait(false);
            return;
        }

        await SendNotFoundAsync(ct).ConfigureAwait(false);
    }
}