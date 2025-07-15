using Moq;
using ThreadPilot.Insurance.Api.ApiClients;
using ThreadPilot.Insurance.Api.Models;
using ThreadPilot.Insurance.Api.Repository;
using ThreadPilot.Insurance.Api.Services;
using ThreadPilot.Shared.Models;

public class InsuranceServiceTests
{
    private readonly Mock<IReadInsuranceRepository> _insuranceRepoMock = new();
    private readonly Mock<IVehicleApiClient> _vehicleApiClientMock = new();
    private readonly InsuranceService _service;

    public InsuranceServiceTests()
    {
        _service = new InsuranceService(_insuranceRepoMock.Object, _vehicleApiClientMock.Object);
    }

    [Fact]
    public async Task ProcessInsurances_ReturnsEmpty_WhenNoInsurances()
    {
        _insuranceRepoMock.Setup(r => r.GetInsurancesByPersonalId(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var result = await _service.ProcessInsurances("id", CancellationToken.None);

        Assert.Equal(InsuranceResponse.Empty, result);
    }

    [Fact]
    public async Task ProcessInsurances_EnrichesVehicleInsurances()
    {
        var vehicleInsurance = new VehicleInsurance
        {
            PersonalId = "1",
            Price = 30,
            Vehicle = new Vehicle { RegistrationNumber = "REGNR2" }
        };
        var insurances = new List<InsuranceBase> { vehicleInsurance };
        var vehicle = new Vehicle { RegistrationNumber = "REGNR2", Manufacturer = "BMW", Year = 2018 };

        _insuranceRepoMock.Setup(r => r.GetInsurancesByPersonalId("1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(insurances);
        
        _vehicleApiClientMock.Setup(v => v.GetVehiclesByRegistrationNumber(It.IsAny<List<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([vehicle]);

        var result = await _service.ProcessInsurances("1", CancellationToken.None);

        Assert.Single(result.Insurances);
        Assert.Equal(vehicle, ((VehicleInsurance)result.Insurances[0]).Vehicle);
    }

    [Fact]
    public async Task ProcessInsurances_HandlesNonVehicleInsurances()
    {
        var healthInsurance = new HealthInsurance { PersonalId = "1", Price = 20 };
        var insurances = new List<InsuranceBase> { healthInsurance };

        _insuranceRepoMock.Setup(r => r.GetInsurancesByPersonalId("1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(insurances);

        var result = await _service.ProcessInsurances("1", CancellationToken.None);

        Assert.Single(result.Insurances);
        Assert.IsType<HealthInsurance>(result.Insurances[0]);
    }

    [Fact]
    public async Task ProcessInsurances_MixedTypes_EnrichesVehicleOnly()
    {
        var vehicleInsurance = new VehicleInsurance
        {
            PersonalId = "1",
            Price = 30,
            Vehicle = new Vehicle { RegistrationNumber = "REGNR2" }
        };
        var healthInsurance = new HealthInsurance { PersonalId = "1", Price = 20 };
        var insurances = new List<InsuranceBase> { vehicleInsurance, healthInsurance };
        var vehicle = new Vehicle { RegistrationNumber = "REGNR2", Manufacturer = "BMW", Year = 2018 };

        _insuranceRepoMock.Setup(r => r.GetInsurancesByPersonalId("1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(insurances);
        _vehicleApiClientMock.Setup(v => v.GetVehiclesByRegistrationNumber(It.IsAny<List<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Vehicle> { vehicle });

        var result = await _service.ProcessInsurances("1", CancellationToken.None);

        Assert.Equal(2, result.Insurances.Length);
        Assert.Equal(vehicle, ((VehicleInsurance)result.Insurances[0]).Vehicle);
        Assert.IsType<HealthInsurance>(result.Insurances[1]);
    }

    [Fact]
    public async Task ProcessInsurances_VehicleInsuranceWithMissingRegistration_DoesNotCallApi()
    {
        var vehicleInsurance = new VehicleInsurance
        {
            PersonalId = "1",
            Price = 30
        };
        var insurances = new List<InsuranceBase> { vehicleInsurance };

        _insuranceRepoMock.Setup(r => r.GetInsurancesByPersonalId("1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(insurances);

        var result = await _service.ProcessInsurances("1", CancellationToken.None);

        _vehicleApiClientMock.Verify(v => v.GetVehiclesByRegistrationNumber(It.IsAny<List<string>>(), It.IsAny<CancellationToken>()), Times.Never);
        Assert.Single(result.Insurances);
    }

    [Fact]
    public async Task ProcessInsurances_VehicleApiReturnsNoVehicles_DoesNotEnrich()
    {
        var vehicleInsurance = new VehicleInsurance
        {
            PersonalId = "1",
            Price = 20,
            Vehicle = new Vehicle { RegistrationNumber = "REGNR2" }
        };
        var insurances = new List<InsuranceBase> { vehicleInsurance };

        _insuranceRepoMock.Setup(r => r.GetInsurancesByPersonalId("1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(insurances);
        _vehicleApiClientMock.Setup(v => v.GetVehiclesByRegistrationNumber(It.IsAny<List<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var result = await _service.ProcessInsurances("1", CancellationToken.None);

        Assert.NotNull(((VehicleInsurance)result.Insurances[0]).Vehicle);
        Assert.Null(((VehicleInsurance)result.Insurances[0]).Vehicle!.Manufacturer);
    }
}