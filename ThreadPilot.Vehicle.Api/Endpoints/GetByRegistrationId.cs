using ThreadPilot.Vehicle.Api.Repository;

namespace ThreadPilot.Vehicle.Api.Endpoints;

public class GetByRegistrationId(IVehicleRepository vehicleRepository) : EndpointWithoutRequest<Shared.Models.Vehicle>
{
    public override void Configure()
    {
        Get("/vehicle/{registrationId}");
        Description(x => x.WithName("GetByRegistrationId"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var registrationId = Route<string>("registrationId");
        if (registrationId is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        
        var vehicle = await vehicleRepository
            .GetVehicleByRegistrationNumber(registrationId, ct)
            .ConfigureAwait(false);

        if (vehicle is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(vehicle, ct);
    }
}