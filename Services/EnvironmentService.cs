namespace HouseHoldeBudgetApi.Services;

public class EnvironmentService
{
    public required string apiKey { get; init; }
    public required string postgresConnectionString { get; init; }
    public required string jwtKey { get; init; }

    public required string passwordSalt { get; init; }
}