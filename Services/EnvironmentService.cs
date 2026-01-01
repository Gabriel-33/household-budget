namespace HouseHoldeBudgetApi.Services;

//MODEL QUE REPRESENTA DADOS DE VARIÁVEIS DE AMBIENTE
public class EnvironmentService
{
    public required string apiKey { get; init; }
    public required string postgresConnectionString { get; init; }
    public required string jwtKey { get; init; }

    public required string passwordSalt { get; init; }
}