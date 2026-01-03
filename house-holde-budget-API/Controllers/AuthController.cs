using FluentValidation;
using FluentValidation.Results;
using HouseHoldeBudgetApi.Exceptions;
using HouseHoldeBudgetApi.Mapper;
using HouseHoldeBudgetApi.Models;
using HouseHoldeBudgetApi.Models.Enums;
using HouseHoldeBudgetApi.Repositories;
using HouseHoldeBudgetApi.Services.Hash;
using HouseHoldeBudgetApi.Services.Jwt;
using HouseHoldeBudgetApi.Validators;
using ILogger = Serilog.ILogger;

namespace HouseHoldeBudgetApi.Controllers;

/// <summary>
/// Implementação genérica de <see cref="IAuthController"/>.
/// </summary>
public class AuthController : IAuthController
{
    private IUsuarioRepository usuarioRepository { get; }
    private UsuarioModelMapper usuarioModelMapper { get; }
    private RegisterUserRequestModelMapper registerUserRequestModelMapper { get; }
    private IJwtService jwtService { get; }
    private IHashService hashService { get; }
    private IValidator<RegisterUserRequestModel> registerUserRequestModelValidator { get; }
    private IValidator<UserLoginRequestModel> userLoginRequestModelValidator { get; }
    private ILogger logger { get; }

    public AuthController(IUsuarioRepository usuarioRepository, UsuarioModelMapper usuarioModelMapper,
        RegisterUserRequestModelMapper registerUserRequestModelMapper, IJwtService jwtService, IHashService hashService,
        IValidator<RegisterUserRequestModel> registerUserRequestModelValidator, IValidator<UserLoginRequestModel> userLoginRequestModelValidator,
        ILogger logger)
    {
        this.usuarioRepository = usuarioRepository;
        this.usuarioModelMapper = usuarioModelMapper;
        this.registerUserRequestModelMapper = registerUserRequestModelMapper;
        this.jwtService = jwtService;
        this.hashService = hashService;
        this.registerUserRequestModelValidator = registerUserRequestModelValidator;
        this.userLoginRequestModelValidator = userLoginRequestModelValidator;
        this.logger = logger;
    }

    /// <summary>
    /// Cadastra um novo usuário. Ele irá validar os campos da requisição, verificar se já existe um usuário com o mesmo código de usuário (matrícula) e email,
    /// relacionar o curso ao usuário, gerar uma senha criptografada, cadastrar o usuário, gerar um token de autenticação e enviar um email de confirmação.
    /// </summary>
    /// <param name="registerUserRequestModel">Modelo usado para transferir as infomaçoes do futuro usuario vindo da requisição.</param>
    /// <returns>Retorna o modelo de leitura (<see cref="UserReadModel"/>) do usuário cadastrado e o token de autenticação.</returns>
    /// <exception cref="ValidationException">Ocorre quando alguma informação contradiz alguma regra de validação.
    /// Regras: <seealso cref="RegisterUserRequestModelValidator"/>.</exception>
    /// <exception cref="ExistsUserException">Ocorre quando já existe algum usuário com a mesma matrícula ou email.</exception>
    /// <exception cref="CursoNotFoundException">Ocorre quando o curso solicitado para relação não existe.</exception>
    public async Task<(UserReadModel, string, int)> RegisterNewUser(RegisterUserRequestModel registerUserRequestModel, HttpContext? httpContext = null)
    {
        logger.Information("Validando campos da requisição de cadastro para Username[{Username}]",
            registerUserRequestModel.username);
        ValidationResult validationResult = await registerUserRequestModelValidator
            .ValidateAsync(registerUserRequestModel);
        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors
                .Select(e => e.ErrorMessage).ToList();
                            
            // Concatena todas as mensagens
            var errorMessage = string.Join("; ", errorMessages);
            
            ValidationException exception = new(errorMessage);
            logger.Error(exception, "Validation issues");
            throw exception;
        }

        logger.Information("Verificando se já existe um usuário com o CodigoUsuario[{CodigoUsuario}] e Email[{Email}]",
            registerUserRequestModel.username, registerUserRequestModel.email);
        bool invalidExists = await usuarioRepository
            .CheckUserByEmail(registerUserRequestModel.email);
        if (invalidExists)
        {
            ExistsUserException exception = new(
                registerUserRequestModel.email
            );
            logger.Error(exception, "Um usuário com o {Email} já existe",
                nameof(registerUserRequestModel.username), nameof(registerUserRequestModel.email));
            throw exception;
        }
        
        string rawUserPassword = registerUserRequestModel.password;
        UsuarioModel usuarioModel = registerUserRequestModelMapper
            .RegisterUserRequestModelToUsuarioModel(registerUserRequestModel);
        usuarioModel.senhaUsuario = hashService.Hash(rawUserPassword);
        usuarioModel.tipoUsuario = UserRole.Admin;        

        logger.Information("Cadastrando usuário Username[{Username}]",
            registerUserRequestModel.username);
        await usuarioRepository.CreateUser(usuarioModel);
        
        await usuarioRepository.FlushChanges();
        
        UserReadModel userReadModel = usuarioModelMapper.UsuarioModelToUserReadModel(usuarioModel);

        logger.Information("Usuário ID[{ID}] cadastrado com sucesso",
        usuarioModel.idUsuario);
        
        var (jwtUser, identity) = jwtService.GenerateJwtAndReturnClaims(new(userReadModel.id.ToString(), userReadModel.role));
        
        return (userReadModel, jwtUser, userReadModel.id);

    }

    /// <summary>
    /// Realiza a autenticação de um usuário. Ele irá validar os campos da requisição, verificar se o usuário existe, verificar se a senha está correta
    /// e gerar o token de autenticação.
    /// </summary>
    /// <param name="userLoginRequestModel">Modelo usado para transferir as infomações de login da requisição para o controller.</param>
    /// <returns>Retorna o modelo de leitura (<see cref="UserReadModel"/>) do usuário cadastrado e o token de autenticação.</returns>
    /// <exception cref="ValidationException">Ocorre quando alguma informação contradiz alguma regra de validação.
    /// Regras: <seealso cref="UserLoginRequestModelValidator"/>.</exception>
    /// <exception cref="UsuarioNotFoundException"></exception>
    /// <exception cref="InvalidLoginPasswordException"></exception>
    public async Task<(UserReadModel, string, int)> LoginUser(UserLoginRequestModel userLoginRequestModel, HttpContext? httpContext = null)
    {
        logger.Information("Validando campos da requisição de login para Email[{UserEmail}]",
            userLoginRequestModel.email);
        ValidationResult validationResult = await userLoginRequestModelValidator
            .ValidateAsync(userLoginRequestModel);
        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
            // Concatena todas as mensagens
            var errorMessage = string.Join("; ", errorMessages);
            
            ValidationException exception = new(errorMessage);
            logger.Error(exception, "Validation issues");
            throw exception;
        }

        logger.Information("Recuperando usuário com Email[{UserEmail}]",
            userLoginRequestModel.email);
        UsuarioModel? usuarioModel = await usuarioRepository
            .GetUsuarioByEmail(userLoginRequestModel.email, true);
        if (usuarioModel is null)
        {
            UsuarioNotFoundException exception = new(nameof(userLoginRequestModel.email), userLoginRequestModel.email);
            logger.Error(exception, "Usuário não encontrado");
            throw exception;
        }

        logger.Information("Verificando senha do usuário Email[{UserEmail}]",
            userLoginRequestModel.email);
        string hashRequestUserPassword = hashService.Hash(userLoginRequestModel.password);
        if (usuarioModel.senhaUsuario != hashRequestUserPassword)
        {
            InvalidLoginPasswordException exception = new(userLoginRequestModel.email);
            logger.Error(exception, "Senha inválida");
            throw exception;
        }

        logger.Information("Gerando token de autenticação para ID[{ID}]",
            usuarioModel.idUsuario);
        UserReadModel userReadModel = usuarioModelMapper.UsuarioModelToUserReadModel(usuarioModel);

        var (jwtUser, identity) = jwtService.GenerateJwtAndReturnClaims(new(userReadModel.id.ToString(), userReadModel.role));
        
        return (userReadModel, jwtUser, userReadModel.id);
    }
}