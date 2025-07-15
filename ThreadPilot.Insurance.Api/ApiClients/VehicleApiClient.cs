using ThreadPilot.Shared.Models;

namespace ThreadPilot.Insurance.Api.ApiClients;

public interface IVehicleApiClient
{
    Task<List<Vehicle>> GetVehiclesByRegistrationNumber(List<string> registrationIds, CancellationToken ct);
}
public class VehicleApiClient(HttpClient httpClient) : IVehicleApiClient
{
    public async Task<List<Vehicle>> GetVehiclesByRegistrationNumber(List<string> registrationIds, CancellationToken ct) {
        var response = await httpClient.GetFromJsonAsync<List<Vehicle>>($"/api/v1/vehicles/{string.Join(",",registrationIds)}",  ct).ConfigureAwait(false);
        return response ?? [];
    }
}