// Endpoints/CategoriaEndpoints.cs
using Microsoft.AspNetCore.Mvc;
using HouseholdBudgetApi.Controllers;
using HouseholdBudgetApi.Exceptions;
using HouseholdBudgetApi.Summaries;
using HouseHoldeBudgetApi.Exceptions;
using HouseHoldeBudgetApi.Middlewares.Auth;
using HouseHoldeBudgetApi.Models;
using HouseHoldeBudgetApi.Models.Requests;
using HouseHoldeBudgetApi.Models.Responses;

namespace HouseholdBudgetApi.Endpoints;

/// <summary>
/// Endpoints para gerenciamento de categorias no sistema de gastos.
/// </summary>
public static class CategoriaEndpoints
{
    /// <summary>
    /// Mapeia os endpoints de categoria.
    /// </summary>
    /// <param name="builder">Grupo de endpoints de categorias <c>/categorias</c>.</param>
    /// <returns></returns>
    public static RouteGroupBuilder MapCategoriaEndpoints(this RouteGroupBuilder builder)
    {
        #region Admin permission
        builder.MapGet("/", GetCategorias)
            .WithOpenApi(GastosSummaries.AdminGetCategoriasSpecification)
            .RequireAuthorization(AuthorizationPolicies.REQUIRE_IDENTIFIER_AND_ADMIN_ROLE);
        
        builder.MapPost("/", PostCreateCategoria)
            .WithOpenApi(GastosSummaries.AdminCreateCategoriaSpecification)
            .RequireAuthorization(AuthorizationPolicies.REQUIRE_IDENTIFIER_AND_ADMIN_ROLE);
        
        builder.MapGet("/finalidade/{finalidade}", GetCategoriasByFinalidade)
            .WithOpenApi(operation =>
            {
                operation.Summary = "Listar categorias por finalidade";
                operation.Description = "Retorna categorias filtradas por finalidade (Despesa, Receita ou Ambas)";
                return operation;
            })
            .RequireAuthorization(AuthorizationPolicies.REQUIRE_IDENTIFIER_AND_ADMIN_ROLE);
        #endregion
        
        return builder;
    }

    #region Admin permission
    /// <summary>
    /// Trata requisição de <c>/categorias/</c>
    /// </summary>
    /// <param name="context">Contexto da requisição</param>
    /// <param name="controller">Controlador que irá gerenciar as necessidades da requisição</param>
    /// <returns>Resposta da requisição</returns>
    /// <permission cref="AuthorizationPolicies">Requisições devem estar autenticadas.
    /// Política: <see cref="AuthorizationPolicies.REQUIRE_IDENTIFIER_AND_ADMIN_ROLE"/></permission>
    [ProducesResponseType(typeof(List<CategoriaReadModel>), 200)]
    private static async Task<IResult> GetCategorias(HttpContext context,
        [FromServices] ICategoriaController controller)
    {
        List<CategoriaReadModel> result;
        try
        {
            result = await controller.GetCategorias();
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }

        return Results.Ok(result);
    }

    /// <summary>
    /// Trata requisição POST de <c>/categorias/</c>
    /// </summary>
    /// <param name="context">Contexto da requisição</param>
    /// <param name="request">Informações da categoria a ser criada</param>
    /// <param name="controller">Controlador que irá gerenciar as necessidades da requisição</param>
    /// <returns>Resposta da requisição</returns>
    [ProducesResponseType(typeof(CategoriaReadModel), 201)]
    private static async Task<IResult> PostCreateCategoria(HttpContext context,
        [FromBody] CreateCategoriaRequestModel request,
        [FromServices] ICategoriaController controller)
    {
        CategoriaReadModel categoriaCriada;

        try
        {
            categoriaCriada = await controller.CreateCategoria(request);
        }
        catch (ValidationException e)
        {
            return Results.BadRequest(e.Message);
        }

        return Results.Created($"/api/categorias/{categoriaCriada.Id}", categoriaCriada);
    }

    /// <summary>
    /// Trata requisição de <c>/categorias/finalidade/{finalidade}</c>
    /// </summary>
    /// <param name="context">Contexto da requisição</param>
    /// <param name="finalidade">Finalidade das categorias (Despesa, Receita ou Ambas)</param>
    /// <param name="controller">Controlador que irá gerenciar as necessidades da requisição</param>
    /// <returns>Resposta da requisição</returns>
    /// <permission cref="AuthorizationPolicies">Requisições devem estar autenticadas.
    /// Política: <see cref="AuthorizationPolicies.REQUIRE_IDENTIFIER_AND_ADMIN_ROLE"/></permission>
    [ProducesResponseType(typeof(List<CategoriaReadModel>), 200)]
    private static async Task<IResult> GetCategoriasByFinalidade(HttpContext context,
        [FromRoute] string finalidade,
        [FromServices] ICategoriaController controller)
    {
        // Validar se a finalidade é um valor válido do enum
        if (!Enum.TryParse<FinalidadeCategoria>(finalidade, true, out var finalidadeEnum))
        {
            return Results.BadRequest($"Finalidade inválida. Use: {string.Join(", ", Enum.GetNames<FinalidadeCategoria>())}");
        }

        List<CategoriaReadModel> result;
        try
        {
            result = await controller.GetCategoriasByFinalidade(finalidadeEnum);
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }

        return Results.Ok(result);
    }
    #endregion
}