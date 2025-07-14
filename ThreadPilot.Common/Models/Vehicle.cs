namespace ThreadPilot.Shared.Models;

public record Vehicle
{
    public string? Manufacturer { get; init; }
    public int? Year { get; init; }
    public string? RegistrationNumber { get; init; }
}