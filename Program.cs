using HouseholdBudgetApi.Endpoints;
using Serilog;
using HouseHoldeBudgetApi.Endpoints;
using HouseHoldeBudgetApi.Endpoints.AuthEndpoints;
using HouseHoldeBudgetApi.Middlewares.Cors;
using HouseHoldeBudgetApi.Middlewares.Filters;
using HouseHoldeBudgetApi.Services;
using ILogger = Serilog.ILogger;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ILogger serilog = new LoggerConfiguration()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.ClearProviders().AddSerilog(serilog);
builder.Host.UseSerilog(serilog);

builder.Services
    .AddApiMetadata()
    .AddServicesConfiguration(builder.Configuration)
    .ConfigureServices()
    .AddOtMetrics()
    .AddCustomCors(builder.Configuration);

builder.Services.AddStorageServices()
    .AddLocalServices()
    .AddMappers()
    .AddValidators()
    .AddApiControllers()
    .AddApiRepositories();

builder.Services.AddAuth();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ========================================
// 3. CORS
// ========================================
app.UseCors(CorsPoliciesName.ALLOW_ALL_CORS_POLICY);


app.MapPrometheusScrapingEndpoint();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();


// ========================================
// Endpoints
// ========================================
RouteGroupBuilder authGroup = app.MapGroup("auth")
    .AddEndpointFilter<ApiKeyFilter>()
    .RequireCors(CorsPoliciesName.ALLOW_ALL_CORS_POLICY)
    .WithTags("Autenticação");
authGroup.MapAuthenticationEndpoints();

RouteGroupBuilder userGroup = app.MapGroup("user")
    .AddEndpointFilter<ApiKeyFilter>()
    .RequireCors(CorsPoliciesName.ALLOW_ALL_CORS_POLICY)
    .WithTags("Usuário");
userGroup.MapUserEndpoints();

RouteGroupBuilder utilsGroup = app.MapGroup("utils")
    .AddEndpointFilter<ApiKeyFilter>()
    .RequireCors(CorsPoliciesName.ALLOW_ALL_CORS_POLICY)
    .WithTags("Utils");
utilsGroup.MapUtilsEndpoints();

RouteGroupBuilder pessoasGroup = app.MapGroup("pessoas")
    .AddEndpointFilter<ApiKeyFilter>()
    .RequireCors(CorsPoliciesName.ALLOW_ALL_CORS_POLICY)
    .WithTags("Pessoas - Sistema de Gastos");
pessoasGroup.MapPessoaEndpoints();

RouteGroupBuilder transacoesGroup = app.MapGroup("transacoes")
    .AddEndpointFilter<ApiKeyFilter>()
    .RequireCors(CorsPoliciesName.ALLOW_ALL_CORS_POLICY)
    .WithTags("Transações - Sistema de Gastos");
transacoesGroup.MapTransacaoEndpoints();

RouteGroupBuilder categoriasGroup = app.MapGroup("categorias")
    .AddEndpointFilter<ApiKeyFilter>()
    .RequireCors(CorsPoliciesName.ALLOW_ALL_CORS_POLICY)
    .WithTags("Categorias - Sistema de Gastos");
categoriasGroup.MapCategoriaEndpoints();

RouteGroupBuilder relatorioGroup = app.MapGroup("relatorios")
    .AddEndpointFilter<ApiKeyFilter>()
    .RequireCors(CorsPoliciesName.ALLOW_ALL_CORS_POLICY)
    .WithTags("Relatórios - Sistema de Gastos");
relatorioGroup.MapRelatorioEndpoints();

app.Run();
