// Endpoints/TransacaoEndpoints.cs
using Microsoft.AspNetCore.Mvc;
using HouseholdBudgetApi.Controllers;
using HouseholdBudgetApi.Exceptions;
using HouseholdBudgetApi.Models;
using HouseholdBudgetApi.Summaries;
using HouseHoldeBudgetApi.Exceptions;
using HouseHoldeBudgetApi.Middlewares.Auth;
using HouseHoldeBudgetApi.Models;
using HouseHoldeBudgetApi.Models.Requests;
using HouseHoldeBudgetApi.Models.Responses;

namespace HouseholdBudgetApi.Endpoints;

/// <summary>
/// Endpoints para gerenciamento de transações no sistema de gastos.
/// </summary>
public static class TransacaoEndpoints
{
    /// <summary>
    /// Mapeia os endpoints de transação.
    /// </summary>
    /// <param name="builder">Grupo de endpoints de transações <c>/transacoes</c>.</param>
    /// <returns></returns>
    public static RouteGroupBuilder MapTransacaoEndpoints(this RouteGroupBuilder builder)
    {
        #region User permission
        builder.MapGet("/", GetTransacoes)
            .WithOpenApi(GastosSummaries.UserGetTransacoesSpecification)
            .RequireAuthorization(AuthorizationPolicies.REQUIRE_IDENTIFIER_AND_USER_ROLE);
        
        builder.MapPost("/", PostCreateTransacao)
            .WithOpenApi(GastosSummaries.UserCreateTransacaoSpecification)
            .RequireAuthorization(AuthorizationPolicies.REQUIRE_IDENTIFIER_AND_USER_ROLE);
        #endregion
        
        return builder;
    }

    #region User permission
    /// <summary>
    /// Trata requisição de <c>/transacoes/</c>
    /// </summary>
    /// <param name="context">Contexto da requisição</param>
    /// <param name="page">Número da página</param>
    /// <param name="pageSize">Tamanho de cada página</param>
    /// <param name="pessoaId">Filtro opcional por ID da pessoa</param>
    /// <param name="tipo">Filtro opcional por tipo (Despesa/Receita)</param>
    /// <param name="controller">Controlador que irá gerenciar as necessidades da requisição</param>
    /// <returns>Resposta da requisição</returns>
    /// <permission cref="AuthorizationPolicies">Requisições devem estar autenticadas.
    /// Política: <see cref="AuthorizationPolicies.REQUIRE_IDENTIFIER_AND_USER_ROLE"/></permission>
    [ProducesResponseType(typeof(UsersListResponse), 200)]
    private static async Task<IResult> GetTransacoes(HttpContext context,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] int? pessoaId,
        [FromQuery] string? tipo,
        [FromServices] ITransacaoController controller)
    {
        TransacoesListResponse result;
        try
        {
            result = await controller.GetTransacoes(page, pageSize, pessoaId, tipo);
        }
        catch (ValidationException e)
        {
            return Results.BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }

        return Results.Ok(result);
    }

    /// <summary>
    /// Trata requisição POST de <c>/transacoes/</c>
    /// </summary>
    /// <param name="context">Contexto da requisição</param>
    /// <param name="request">Informações da transação a ser criada</param>
    /// <param name="controller">Controlador que irá gerenciar as necessidades da requisição</param>
    /// <returns>Resposta da requisição</returns>
    [ProducesResponseType(typeof(TransacaoReadModel), 201)]
    private static async Task<IResult> PostCreateTransacao(HttpContext context,
        [FromBody] CreateTransacaoRequestModel request,
        [FromServices] ITransacaoController controller)
    {
        TransacaoReadModel transacaoCriada;

        try
        {
            transacaoCriada = await controller.CreateTransacao(request);
        }
        catch (ValidationException e)
        {
            return Results.BadRequest(e.Message);
        }
        catch (PessoaNotFoundException e)
        {
            return Results.NotFound(e.Message);
        }
        catch (CategoriaNotFoundException e)
        {
            return Results.NotFound(e.Message);
        }

        return Results.Created($"/api/transacoes/{transacaoCriada.Id}", transacaoCriada);
    }
    #endregion
}