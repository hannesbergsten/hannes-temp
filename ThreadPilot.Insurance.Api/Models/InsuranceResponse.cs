namespace ThreadPilot.Insurance.Api.Models;

public record InsuranceResponse
{
    public decimal TotalPrice { get; init; }
    public required InsuranceBase[] Insurances { get; init; } = [];
}