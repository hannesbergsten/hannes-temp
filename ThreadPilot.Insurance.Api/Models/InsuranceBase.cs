using System.Text.Json.Serialization;

namespace ThreadPilot.Insurance.Api.Models;


[JsonDerivedType(typeof(VehicleInsurance), typeDiscriminator: (int)InsuranceType.Vehicle)]
[JsonDerivedType(typeof(PetInsurance), typeDiscriminator: (int)InsuranceType.Pet)]
[JsonDerivedType(typeof(HealthInsurance), typeDiscriminator: (int)InsuranceType.Health)]
public abstract record InsuranceBase
{
    public abstract InsuranceType Type { get; }
    public abstract string InsuranceName { get; }  
    public required string PersonalId { get; init; }
    public required decimal Price { get; init; }
}