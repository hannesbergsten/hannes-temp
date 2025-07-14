using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ThreadPilot.Insurance.Api.ApiClients;
using ThreadPilot.Insurance.Api.Models;
using ThreadPilot.Insurance.Api.Test.Integration.Helpers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using Xunit;

namespace ThreadPilot.Insurance.Api.Test.Integration.Endpoints
{
    public class GetInsurancesByIdTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private const string ValidId = "1";
        private const string InValidId = "999";
        private readonly HttpClient _httpClient;
        
        public GetInsurancesByIdTests(WebApplicationFactory<Program> factory)
        {
            var customFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IVehicleApiClient, TestVehicleApiClient>();
                });
            });

            _httpClient = customFactory.CreateClient();
        }
        
        [Fact]
        public async Task GetInsuranceById_ReturnsOk_WithValidId()
        {
            var response = await _httpClient.GetAsync($"/api/v1/insurances/{ValidId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var insuranceResponse = await response.Content.ReadFromJsonAsync<InsuranceResponse>();
            Assert.NotNull(insuranceResponse);
            Assert.Equal(50, insuranceResponse.TotalPrice);
            Assert.Collection(insuranceResponse.Insurances,
                insurance =>
                {
                    Assert.Equal("Car insurance", insurance.InsuranceName);
                    Assert.Equal(30, insurance.Price);
                    Assert.Equal("1", insurance.PersonalId);
                    Assert.Equal(InsuranceType.Vehicle, insurance.Type);
                    var vehicle = ((VehicleInsurance)insurance).Vehicle;
                    Assert.NotNull(vehicle);
                    Assert.Equal("REGNR2", vehicle.RegistrationNumber);
                    Assert.Equal("BMW", vehicle.Manufacturer);
                    Assert.Equal(2018, vehicle.Year);
                },
                insurance =>
                {
                    Assert.Equal("Personal health insurance", insurance.InsuranceName);
                    Assert.Equal(20, insurance.Price);
                    Assert.Equal("1", insurance.PersonalId);
                    Assert.Equal(InsuranceType.Health, insurance.Type);
                });
        }

        [Fact]
        public async Task GetInsuranceById_ReturnsNotFound_WithInvalidId()
        {
            var response = await _httpClient.GetAsync($"/api/v1/insurances/{InValidId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        
        [Theory]
        [InlineData("abd")]
        [InlineData("1234")]
        public async Task GetInsuranceById_ReturnsBadRequest_WithInvalidIdFormat(string invalidId)
        {
            var response = await _httpClient.GetAsync($"/api/v1/insurances/{invalidId}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetInsuranceById_ReturnsNotFound_WithValidIdButNoInsurances()
        {
            var response = await _httpClient.GetAsync($"/api/v1/insurances/5"); // id not present in repo
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetInsuranceById_ReturnsSingleInsuranceType()
        {
            var response = await _httpClient.GetAsync($"/api/v1/insurances/3");
            var insuranceResponse = await response.Content.ReadFromJsonAsync<InsuranceResponse>();
            Assert.Single(insuranceResponse!.Insurances);
            Assert.IsType<PetInsurance>(insuranceResponse.Insurances[0]);
        }
    }
}