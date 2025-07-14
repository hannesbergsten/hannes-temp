using ThreadPilot.Shared.Models;

namespace ThreadPilot.Insurance.Api.ApiClients;

public interface IVehicleApiClient
{
    Task<Vehicle?> GetVehicleByRegistrationNumber(string registrationId, CancellationToken ct);
}
public class VehicleApiClient(HttpClient httpClient) : IVehicleApiClient
{
    public async Task<Vehicle?> GetVehicleByRegistrationNumber(string registrationId, CancellationToken ct) {
        var response = await httpClient.GetFromJsonAsync<Vehicle>($"/api/v1/vehicle/{registrationId}",  ct).ConfigureAwait(false);
        return response;
    }
}