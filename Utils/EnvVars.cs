using HouseHoldeBudgetApi.Services;

namespace HouseHoldeBudgetApi.Utils;

internal static class EnvVars
{
    //TODO: Move the API Key too
    private const string API_KEY = "API_KEY";
    private const string POSTGRES_CONNECTION_STRING = "POSTGRES_CONNECTION_STRING";
    private const string JWT_KEY = "JWT_KEY";
    private const string PASSWORD_SALT = "PASSWORD_SALT";
    private const string SUPABASE_URL = "SUPA_URL";
    private const string SUPABASE_KEY = "SUPA_KEY";
        
    public static string? GetApiKey() =>
        Environment.GetEnvironmentVariable(API_KEY);
    private static string? GetPostgresConnectionString() =>
        Environment.GetEnvironmentVariable(POSTGRES_CONNECTION_STRING);
    private static string? GetJwtKey() =>
        Environment.GetEnvironmentVariable(JWT_KEY);
    
    private static string? GetPasswordSalt() =>
        Environment.GetEnvironmentVariable(PASSWORD_SALT);
    
    public static string? GetSupabaseUrl() =>
        Environment.GetEnvironmentVariable(SUPABASE_URL);
    
    public static string? GetSupabaseKey() =>
            Environment.GetEnvironmentVariable(SUPABASE_KEY);

    public static EnvironmentService CreateEnvironmentServiceFromVariables() =>
        new()
        {
            apiKey = GetApiKey() ?? throw new ArgumentNullException(API_KEY),
            postgresConnectionString = GetPostgresConnectionString() ?? throw new ArgumentNullException(POSTGRES_CONNECTION_STRING),
            jwtKey = GetJwtKey() ?? throw new ArgumentNullException(JWT_KEY),
            passwordSalt = GetPasswordSalt() ?? throw new ArgumentNullException(PASSWORD_SALT),
            supabaseUrl = GetSupabaseUrl() ?? throw new ArgumentNullException(SUPABASE_URL),
            supabaseKey = GetSupabaseKey() ?? throw new ArgumentNullException(SUPABASE_KEY)
        };
}