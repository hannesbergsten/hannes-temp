namespace ThreadPilot.Shared.Models;

public record Vehicle
{
    public required string RegistrationNumber { get; init; }
    public string? Manufacturer { get; init; }
    public int? Year { get; init; }
}