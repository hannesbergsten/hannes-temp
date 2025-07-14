using ThreadPilot.Insurance.Api.Models;
using ThreadPilot.Shared.Models;

namespace ThreadPilot.Insurance.Api.Test.Unit;

public class InsuranceTests
{
    [Fact]
    public void InsuranceResponse_Should_Set_Properties_Correctly()
    {
        var vehicleInsurance = GetVehicleInsurance();
        var healthInsurance = GetHealthInsurance();
        var petInsurance = GetPetInsurance();
        
        var insurances = new InsuranceBase[] { vehicleInsurance, healthInsurance, petInsurance };
        var expectedTotalPrice = vehicleInsurance.Price + healthInsurance.Price + petInsurance.Price;

        var response = new InsuranceResponse
        {
            Insurances = insurances,
            TotalPrice = expectedTotalPrice
        };

        Assert.Equal(expectedTotalPrice, response.TotalPrice);
        Assert.Equal(insurances, response.Insurances);
        Assert.Equal(3, response.Insurances.Length);
        Assert.Contains(vehicleInsurance, response.Insurances);
        Assert.Contains(healthInsurance, response.Insurances);
        Assert.Contains(petInsurance, response.Insurances);
    }

    [Fact]
    public void InsuranceResponse_Should_Handle_Empty_Insurances()
    {
        var response = new InsuranceResponse
        {
            Insurances = Array.Empty<InsuranceBase>(),
            TotalPrice = 0
        };
        Assert.Equal(0, response.TotalPrice);
        Assert.Empty(response.Insurances);
    }

    [Fact]
    public void InsuranceResponse_Should_Handle_Single_Insurance()
    {
        var healthInsurance = GetHealthInsurance();
        var response = new InsuranceResponse
        {
            Insurances = new[] { healthInsurance },
            TotalPrice = healthInsurance.Price
        };
        Assert.Single(response.Insurances);
        Assert.Equal(healthInsurance.Price, response.TotalPrice);
    }

    [Fact]
    public void Insurance_Should_Have_Correct_Types()
    {
        var vehicleInsurance = GetVehicleInsurance();
        var healthInsurance = GetHealthInsurance();
        var petInsurance = GetPetInsurance();

        Assert.Equal(InsuranceType.Vehicle, vehicleInsurance.Type);
        Assert.Equal(InsuranceType.Health, healthInsurance.Type);
        Assert.Equal(InsuranceType.Pet, petInsurance.Type);
    }

    private static VehicleInsurance GetVehicleInsurance()
    {
        return new VehicleInsurance
        {
            PersonalId = "123",
            Price = 100,
            Vehicle = new Vehicle
            {
                RegistrationNumber = "REG123",
                Manufacturer = "Tesla",
                Year = 2022
            }
        };
    }

    private static HealthInsurance GetHealthInsurance()
    {
        return new HealthInsurance
        {
            PersonalId = "456",
            Price = 200
        };
    }

    private static PetInsurance GetPetInsurance()
    {
        return new PetInsurance
        {
            PersonalId = "789",
            Price = 50
        };
    }
}