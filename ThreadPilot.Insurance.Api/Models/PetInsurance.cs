namespace ThreadPilot.Insurance.Api.Models;

public record PetInsurance : InsuranceBase
{
    public override InsuranceType Type => InsuranceType.Pet;
    public override string InsuranceName => "Pet insurance";

}