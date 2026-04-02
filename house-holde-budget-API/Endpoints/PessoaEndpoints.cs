// Endpoints/PessoaEndpoints.cs
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
/// Endpoints para gerenciamento de pessoas no sistema de gastos.
/// </summary>
public static class PessoaEndpoints
{
    /// <summary>
    /// Mapeia os endpoints de pessoa.
    /// </summary>
    /// <param name="builder">Grupo de endpoints de pessoas <c>/pessoas</c>.</param>
    /// <returns></returns>
    public static RouteGroupBuilder MapPessoaEndpoints(this RouteGroupBuilder builder)
    {
        #region Admin permission
        builder.MapGet("/", GetPessoas)
            .WithOpenApi(GastosSummaries.AdminGetPessoasSpecification)
            .RequireAuthorization(AuthorizationPolicies.REQUIRE_IDENTIFIER_AND_ADMIN_ROLE);
        
        builder.MapPost("/", PostCreatePessoa)
            .WithOpenApi(GastosSummaries.AdminCreatePessoaSpecification)
            .RequireAuthorization(AuthorizationPolicies.REQUIRE_IDENTIFIER_AND_ADMIN_ROLE);
        
        builder.MapDelete("/{id:int}", DeletePessoa)
            .WithOpenApi(GastosSummaries.AdminDeletePessoaSpecification)
            .RequireAuthorization(AuthorizationPolicies.REQUIRE_IDENTIFIER_AND_ADMIN_ROLE);
        #endregion
        
        return builder;
    }

    #region Admin permission
    /// <summary>
    /// Trata requisição de <c>/pessoas/</c>
    /// </summary>
    /// <param name="context">Contexto da requisição</param>
    /// <param name="page">Número da página</param>
    /// <param name="pageSize">Tamanho de cada página</param>
    /// <param name="controller">Controlador que irá gerenciar as necessidades da requisição</param>
    /// <returns>Resposta da requisição</returns>
    /// <permission cref="AuthorizationPolicies">Requisições devem estar autenticadas.
    /// Política: <see cref="AuthorizationPolicies.REQUIRE_IDENTIFIER_AND_ADMIN_ROLE"/></permission>
    [ProducesResponseType(typeof(UsersListResponse), 200)]
    private static async Task<IResult> GetPessoas(HttpContext context,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromServices] IPessoaController controller)
    {
        PessoasListResponse result;
        try
        {
            result = await controller.GetPessoas(page, pageSize);
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }

        return Results.Ok(result);
    }

    /// <summary>
    /// Trata requisição POST de <c>/pessoas/</c>
    /// </summary>
    /// <param name="context">Contexto da requisição</param>
    /// <param name="request">Informações da pessoa a ser criada</param>
    /// <param name="controller">Controlador que irá gerenciar as necessidades da requisição</param>
    /// <returns>Resposta da requisição</returns>
    [ProducesResponseType(typeof(PessoaReadModel), 201)]
    private static async Task<IResult> PostCreatePessoa(HttpContext context,
        [FromBody] CreatePessoaRequestModel request,
        [FromServices] IPessoaController controller)
    {
        PessoaReadModel pessoaCriada;

        try
        {
            pessoaCriada = await controller.CreatePessoa(request);
        }
        catch (ValidationException e)
        {
            return Results.BadRequest(e.Message);
        }

        return Results.Created($"/api/pessoas/{pessoaCriada.Id}", pessoaCriada);
    }

    /// <summary>
    /// Trata requisição de <c>/pessoas/:id</c>
    /// </summary>
    /// <param name="context">Contexto da requisição</param>
    /// <param name="id">ID da pessoa</param>
    /// <param name="controller">Controlador que irá gerenciar as necessidades da requisição</param>
    /// <returns>Resposta da requisição</returns>
    /// <permission cref="AuthorizationPolicies">Requisições devem estar autenticadas.
    /// Política: <see cref="AuthorizationPolicies.REQUIRE_IDENTIFIER_AND_ADMIN_ROLE"/></permission>
    [ProducesResponseType(typeof(int), 200)]
    private static async Task<IResult> DeletePessoa(HttpContext context,
        [FromRoute] int id,
        [FromServices] IPessoaController controller)
    {
        int deletedId;

        try
        {
            deletedId = await controller.DeletePessoa(id);
        }
        catch (ValidationException e)
        {
            return Results.BadRequest(e.Message);
        }
        catch (PessoaNotFoundException e)
        {
            return Results.NotFound(e.Message);
        }
        
        return Results.Ok(deletedId);
    }
    #endregion
}