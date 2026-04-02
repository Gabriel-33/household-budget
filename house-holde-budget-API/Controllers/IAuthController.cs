using HouseHoldeBudgetApi.Repositories;
using HouseHoldeBudgetApi.Services.Jwt;
using HouseHoldeBudgetApi.Validators;
using HouseHoldeBudgetApi.Models;

namespace HouseHoldeBudgetApi.Controllers;

/// <summary>
/// Controlador de autenticação. Ele é responsável por gerenciar as requisições de autenticação.
/// Nele, é possível realizar o registro de um novo usuário e o login de um usuário já existente.
/// Deve ser implementado e cadastrado no container de DI.
/// </summary>
/// <remarks>
/// Deve usar os serviços basicos do container de DI para realizar as operações de autenticação, como
/// <see cref="ILogger"/>, <see cref="IUsuarioRepository"/>, <see cref="ICursoRepository"/>, <see cref="JwtService"/>. 
/// </remarks>
public interface IAuthController
{
    /// <summary>
    /// Cadastra um novo usuario no sistema.
    /// Os campos recebidos em <paramref name="registerUserRequestModel"/> são validados de acordo com<see cref="RegisterUserRequestModelValidator"/>.
    /// O ID do curso recebido em <paramref name="registerUserRequestModel"/> é usado para relacionar o usuário com o curso,
    /// portanto o curso com este ID deve já estar cadastrado.
    /// </summary>
    /// <param name="registerUserRequestModel">Informações do usuário para cadastro</param>
    /// <returns>Representa uma tarefa assíncrona, ela retorna uma tupla contendo as informações cadastradas no banco
    /// e um JWT válido para o usuário.</returns>
    /// <exception cref="CursoNotFoundException"><c>codeCurso</c> de <paramref name="registerUserRequestModel"/>
    /// não pertence a nenhum curso.</exception>
    public Task<(UserReadModel, string, int)> RegisterNewUser(RegisterUserRequestModel registerUserRequestModel, HttpContext? httpContext);
    /// <summary>
    /// Realiza o reenvio do código de ativação do usuário, para o email do mesmo.
    /// Os campos recebidos em &lt;paramref name="registerUserRequestModel"/&gt; são validados de acordo com <see cref="UserLoginRequestModelValidator"/>
    /// Nenhuma mudança no banco é feita, apenas resgata as informações do usuário, faz verificações de segurança
    /// </summary>
    /// <param name="userLoginRequestModel">Informações de um usuário já cadastrado.</param>
    /// <returns>Representa uma tarefa assíncrona, ela um inteiro com a informação referente ao Id do usuário</returns>
    /// 

    public Task<(UserReadModel, string, int)> LoginUser(UserLoginRequestModel userLoginRequestModel, HttpContext? httpContext);
}