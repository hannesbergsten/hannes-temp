using ThreadPilot.Shared.Models;

namespace ThreadPilot.Insurance.Api.ApiClients;

public interface IVehicleApiClient
{
    Task<IReadOnlyList<Vehicle>> GetVehiclesByRegistrationNumber(IReadOnlyList<string> registrationIds, CancellationToken ct);
}
public class VehicleApiClient(HttpClient httpClient) : IVehicleApiClient
{
    public async Task<IReadOnlyList<Vehicle>> GetVehiclesByRegistrationNumber(IReadOnlyList<string> registrationIds, CancellationToken ct) {
        var response = await httpClient.GetFromJsonAsync<IReadOnlyList<Vehicle>>($"/api/v1/vehicles/{string.Join(",",registrationIds)}",  ct).ConfigureAwait(false);
        return response ?? [];
    }
}