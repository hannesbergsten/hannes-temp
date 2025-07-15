namespace ThreadPilot.Insurance.Api.Models;

public record InsuranceResponse
{
    public decimal TotalPrice { get; init; } = 0;
    public required InsuranceBase[] Insurances { get; init; } = [];

    public static InsuranceResponse Empty => new() { Insurances = [] };
}