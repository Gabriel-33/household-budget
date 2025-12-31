using Microsoft.AspNetCore.Mvc;
using HouseHoldeBudgetApi.Controllers;
using HouseHoldeBudgetApi.Exceptions;
using HouseHoldeBudgetApi.Middlewares.Auth;
using HouseHoldeBudgetApi.Models;
using HouseHoldeBudgetApi.Summaries;

namespace HouseHolde.Endpoints.AuthEndpoints;

public static class AuthAnonEndpoints
{
    public static RouteGroupBuilder MapAnonAuthEndpoints(this RouteGroupBuilder builder)
    {
        builder.MapPost("register", AuthRegisterEndpointHandler)
            .WithOpenApi(AuthSummaries.AuthRegisterSpecification);
        builder.MapPost("login", AuthLoginEndpointHandler)
            .WithOpenApi(AuthSummaries.AuthLoginSpecification);

        return builder;
    }

    #region Register/Login

    /// <summary>
    /// Trata requisição de <c>/auth/register</c>
    /// </summary>
    /// <param name="registerUserRequest">Informações para cadastro do novo usuário.</param>
    /// <param name="controller">Controlador que irá gerenciar as necessidades da requisição.</param>
    /// <returns>Resposta da requisição.</returns>
    /// <permission cref="AuthorizationPolicies">Requisições não autenticadas são autorizadas.</permission>
    async private static Task<IResult> AuthRegisterEndpointHandler(
        HttpContext _httpContext,
        [FromBody] RegisterUserRequestModel registerUserRequest,
        [FromServices] IAuthController controller)
    {
        string jwtNewUser = null;
        int userId = 0;

        try
        {
            (UserReadModel _, jwtNewUser, userId) = await controller.RegisterNewUser(registerUserRequest, _httpContext);
        }
        catch (Exception e)
        {
            return Results.BadRequest(new { message = e.Message, tipo = 2 });
        }

        object retorno = new
        {
            tokenJwt = jwtNewUser,
            idUsuario = userId
        };

        return Results.Json(retorno);
    }
    
    /// <summary>
    /// Trata requisição de <c>/auth/login</c>
    /// </summary>
    /// <param name="loginRequestModel">Informações do usuário para login, vindas do body da requisição.</param>
    /// <param name="controller">Controlador que irá gerenciar as necessidades da requisição.</param>
    /// <returns>Resposta da requisição.</returns>
    /// <permission cref="AuthorizationPolicies">Requisições não autenticadas são autorizadas.</permission>
    async private static Task<IResult> AuthLoginEndpointHandler(
        HttpContext _httpContext,
        [FromBody] UserLoginRequestModel loginRequestModel,
        [FromServices] IAuthController controller)
    {
        string jwtUser;
        int userId = 0;

        try
        {
            (UserReadModel _, jwtUser, userId) = await controller.LoginUser(loginRequestModel, _httpContext);
        }
        catch (UsuarioNotFoundException e)
        {
            return Results.NotFound(e.Message);
        }
        catch (InvalidLoginPasswordException)
        {
            return Results.Unauthorized();
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }

        object retorno = new
        {
            tokenJwt = jwtUser,
            idUsuario = userId
        };

        return Results.Json(retorno);
    }

    #endregion
}