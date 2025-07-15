using ThreadPilot.Vehicle.Api.Repository;

namespace ThreadPilot.Vehicle.Api.Endpoints;

public class GetByRegistrationId(IVehicleRepository vehicleRepository) : EndpointWithoutRequest<List<Shared.Models.Vehicle>>
{
    public override void Configure()
    {
        Get("/vehicles/{registrationIds}");
        Description(x => x.WithName("GetByRegistrationId"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var registrationIds = Route<List<string>>("registrationIds");
        if (registrationIds is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        
        var vehicles = await vehicleRepository
            .GetVehiclesByRegistrationNumber(registrationIds, ct)
            .ConfigureAwait(false);

        await SendOkAsync(vehicles, ct);
    }
}