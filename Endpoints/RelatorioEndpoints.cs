// Endpoints/RelatorioEndpoints.cs
using Microsoft.AspNetCore.Mvc;
using HouseholdBudgetApi.Controllers;
using HouseholdBudgetApi.Summaries;
using HouseHoldeBudgetApi.Middlewares.Auth;
using HouseHoldeBudgetApi.Models.Responses;

namespace HouseholdBudgetApi.Endpoints;

/// <summary>
/// Endpoints para geração de relatórios no sistema de gastos.
/// </summary>
public static class RelatorioEndpoints
{
    /// <summary>
    /// Mapeia os endpoints de relatório.
    /// </summary>
    /// <param name="builder">Grupo de endpoints de relatórios <c>/relatorios</c>.</param>
    /// <returns></returns>
    public static RouteGroupBuilder MapRelatorioEndpoints(this RouteGroupBuilder builder)
    {
        #region Admin permission
        builder.MapGet("/pessoas", GetTotaisPorPessoa)
            .WithOpenApi(GastosSummaries.AdminGetRelatorioPessoasSpecification)
            .RequireAuthorization(AuthorizationPolicies.REQUIRE_IDENTIFIER_AND_ADMIN_ROLE);
        
        builder.MapGet("/categorias", GetTotaisPorCategoria)
            .WithOpenApi(GastosSummaries.AdminGetRelatorioCategoriasSpecification)
            .RequireAuthorization(AuthorizationPolicies.REQUIRE_IDENTIFIER_AND_ADMIN_ROLE);
        #endregion
        
        return builder;
    }

    #region Admin permission
    /// <summary>
    /// Trata requisição de <c>/relatorios/pessoas</c>
    /// </summary>
    /// <param name="context">Contexto da requisição</param>
    /// <param name="controller">Controlador que irá gerenciar as necessidades da requisição</param>
    /// <returns>Resposta da requisição</returns>
    /// <permission cref="AuthorizationPolicies">Requisições devem estar autenticadas.
    /// Política: <see cref="AuthorizationPolicies.REQUIRE_IDENTIFIER_AND_ADMIN_ROLE"/></permission>
    [ProducesResponseType(typeof(RelatorioPessoasResponse), 200)]
    private static async Task<IResult> GetTotaisPorPessoa(HttpContext context,
        [FromServices] IRelatorioController controller)
    {
        RelatorioPessoasResponse result;
        try
        {
            result = await controller.GetTotaisPorPessoa();
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }

        return Results.Ok(result);
    }

    /// <summary>
    /// Trata requisição de <c>/relatorios/categorias</c>
    /// </summary>
    /// <param name="context">Contexto da requisição</param>
    /// <param name="controller">Controlador que irá gerenciar as necessidades da requisição</param>
    /// <returns>Resposta da requisição</returns>
    /// <permission cref="AuthorizationPolicies">Requisições devem estar autenticadas.
    /// Política: <see cref="AuthorizationPolicies.REQUIRE_IDENTIFIER_AND_ADMIN_ROLE"/></permission>
    [ProducesResponseType(typeof(RelatorioCategoriasResponse), 200)]
    private static async Task<IResult> GetTotaisPorCategoria(HttpContext context,
        [FromServices] IRelatorioController controller)
    {
        RelatorioCategoriasResponse result;
        try
        {
            result = await controller.GetTotaisPorCategoria();
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }

        return Results.Ok(result);
    }
    #endregion
}