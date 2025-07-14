using ThreadPilot.Shared.Models;

namespace ThreadPilot.Insurance.Api.Models;

public record VehicleInsurance : InsuranceBase
{
    public override InsuranceType Type => InsuranceType.Vehicle;
    public override string InsuranceName => "Car insurance";
    public Vehicle? Vehicle { get; init; }
}