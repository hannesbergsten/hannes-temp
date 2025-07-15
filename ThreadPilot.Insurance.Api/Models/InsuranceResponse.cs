using System.Collections.Immutable;

namespace ThreadPilot.Insurance.Api.Models;

public record InsuranceResponse
{
    public decimal TotalPrice { get; init; } = 0;
    public required ImmutableArray<InsuranceBase> Insurances { get; init; } = [];

    public static InsuranceResponse Empty => new() { Insurances = [] };
}