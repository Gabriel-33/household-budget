using HouseHoldeBudgetApi.Services;

namespace HouseHoldeBudgetApi.Utils;

internal static class EnvVars
{
    //CONFIGURAÇÃO DE VÁRIAVEIS DE AMBIENTE
    private const string API_KEY = "API_KEY";
    private const string POSTGRES_CONNECTION_STRING = "POSTGRES_CONNECTION_STRING";
    private const string JWT_KEY = "JWT_KEY";
    private const string PASSWORD_SALT = "PASSWORD_SALT";
        
    public static string? GetApiKey() =>
        Environment.GetEnvironmentVariable(API_KEY);
    private static string? GetPostgresConnectionString() =>
        Environment.GetEnvironmentVariable(POSTGRES_CONNECTION_STRING);
    private static string? GetJwtKey() =>
        Environment.GetEnvironmentVariable(JWT_KEY);
    
    private static string? GetPasswordSalt() =>
        Environment.GetEnvironmentVariable(PASSWORD_SALT);

    public static EnvironmentService CreateEnvironmentServiceFromVariables() =>
        new()
        {
            apiKey = GetApiKey() ?? throw new ArgumentNullException(API_KEY),
            postgresConnectionString = GetPostgresConnectionString() ?? throw new ArgumentNullException(POSTGRES_CONNECTION_STRING),
            jwtKey = GetJwtKey() ?? throw new ArgumentNullException(JWT_KEY),
            passwordSalt = GetPasswordSalt() ?? throw new ArgumentNullException(PASSWORD_SALT),
        };
}