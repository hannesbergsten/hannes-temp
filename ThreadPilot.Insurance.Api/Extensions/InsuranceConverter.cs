using ThreadPilot.Insurance.Api.Models;

namespace ThreadPilot.Insurance.Api.Extensions;

public static class InsuranceConverter
{
    public static InsuranceResponse ToResponse(this InsuranceBase[] insurances)
    {
        return new InsuranceResponse
        {
            TotalPrice = insurances.Sum(i => i.Price),
            Insurances = insurances
        };
    }
}