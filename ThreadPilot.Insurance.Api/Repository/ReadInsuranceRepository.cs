using ThreadPilot.Insurance.Api.Models;
using ThreadPilot.Shared.Models;

namespace ThreadPilot.Insurance.Api.Repository;

public interface IReadInsuranceRepository
{
    Task<List<InsuranceBase>> GetInsurancesByPersonalId(string personalId, CancellationToken ct);
}

public class ReadInsuranceRepository : IReadInsuranceRepository
{
    public async Task<List<InsuranceBase>> GetInsurancesByPersonalId(string personalId, CancellationToken ct)
    {
        var insurances = new List<InsuranceBase>
        {
            new VehicleInsurance
            {
                PersonalId = "1",
                Price = 30,
                Vehicle = new Vehicle
                {
                    RegistrationNumber = "REGNR2"
                }
            },
            new HealthInsurance
            {
                PersonalId = "2",
                Price = 20
            },
            new HealthInsurance
            {
                PersonalId = "1",
                Price = 20
            },
            new PetInsurance
            {
                PersonalId = "3",
                Price = 10
            }
        };
        return await Task.FromResult(insurances.Where(i => i.PersonalId == personalId).ToList());
    }
}