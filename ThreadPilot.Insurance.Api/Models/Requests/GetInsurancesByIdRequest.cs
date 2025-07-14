namespace ThreadPilot.Insurance.Api.Models.Requests;

public record GetInsurancesByIdRequest
{
    public required string Id { get; init; }
}