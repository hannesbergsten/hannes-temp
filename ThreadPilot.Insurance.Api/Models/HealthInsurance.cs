namespace ThreadPilot.Insurance.Api.Models;

public record HealthInsurance : InsuranceBase
{
    public override InsuranceType Type => InsuranceType.Health;
    public override string InsuranceName => "Personal health insurance";
}