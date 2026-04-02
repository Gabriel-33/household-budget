// Controllers/PessoaController.cs
using FluentValidation;
using FluentValidation.Results;
using HouseholdBudgetApi.Models;
using HouseholdBudgetApi.Repositories;
using HouseHoldeBudgetApi.Exceptions;
using HouseHoldeBudgetApi.Mapper;
using HouseHoldeBudgetApi.Models;
using HouseHoldeBudgetApi.Models.Requests;
using HouseHoldeBudgetApi.Models.Responses;
using HouseHoldeBudgetApi.Validators.CustomValidators;
using ILogger = Serilog.ILogger;
using ValidationException = FluentValidation.ValidationException;

namespace HouseholdBudgetApi.Controllers;

/// <summary>
/// Implementação do controlador de pessoas no sistema de gastos.
/// </summary>
public class PessoaController : IPessoaController
{
    private readonly IPessoaRepository _pessoaRepository;
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly IValidator<CreatePessoaRequestModel> _createPessoaValidator;
    private readonly PessoaModelMapper _pessoaModelMapper;
    private readonly ILogger _logger;

    public PessoaController(
        IPessoaRepository pessoaRepository,
        ITransacaoRepository transacaoRepository,
        IValidator<CreatePessoaRequestModel> createPessoaValidator,
        PessoaModelMapper pessoaModelMapper,
        ILogger logger)
    {
        _pessoaRepository = pessoaRepository;
        _transacaoRepository = transacaoRepository;
        _createPessoaValidator = createPessoaValidator;
        _pessoaModelMapper = pessoaModelMapper;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todas as pessoas cadastradas com paginação.
    /// </summary>
    // Controllers/PessoaController.cs - Modifique o método GetPessoas
    public async Task<PessoasListResponse> GetPessoas(int page, int pageSize)
    {
        _logger.Information("Recuperando pessoas - Page[{Page}] PageSize[{PageSize}]", page, pageSize);
    
        (var pessoas, int resultCount, int totalCount) = await _pessoaRepository.GetPessoasAndCount(page, pageSize);

        var pessoasRead = pessoas
            .Select(_pessoaModelMapper.PessoaToPessoaReadModel)
            .ToList();

        int maxPage = (int)Math.Ceiling((double)totalCount / pageSize);

        return new PessoasListResponse
        {
            maxPage = maxPage,
            totalCount = totalCount,
            pageCount = resultCount,
            items = pessoasRead
        };
    }

    /// <summary>
    /// Cria uma nova pessoa no sistema.
    /// </summary>
    public async Task<PessoaReadModel> CreatePessoa(CreatePessoaRequestModel request)
    {
        _logger.Information("Validando requisição para criar pessoa: Nome[{Nome}] Idade[{Idade}]",
            request.Nome, request.Idade);

        ValidationResult validationResult = await _createPessoaValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            ValidationException exception = new(validationResult.Errors.Select(e => e.ErrorMessage).ToString());
            _logger.Error(exception, "Requisição para criar pessoa inválida");
            throw exception;
        }

        // Validação adicional para idade
        if (request.Idade < 0 || request.Idade > 150)
        {
            ValidationException exception = new("Idade deve estar entre 0 e 150 anos");
            _logger.Error(exception, "Idade inválida para criação de pessoa");
            throw exception;
        }

        _logger.Information("Criando nova pessoa: Nome[{Nome}]", request.Nome);
        
        var pessoa = _pessoaModelMapper.CreatePessoaRequestModelToPessoa(request);

        await _pessoaRepository.CreatePessoa(pessoa);
        await _pessoaRepository.FlushChanges();

        _logger.Information("Pessoa criada com ID[{Id}]", pessoa.Id);

        return _pessoaModelMapper.PessoaToPessoaReadModel(pessoa);
    }

    /// <summary>
    /// Deleta uma pessoa e todas as suas transações (cascade).
    /// </summary>
    public async Task<int> DeletePessoa(int id)
    {
        _logger.Information("Validando ID da pessoa para deleção: ID[{Id}]", id);
        
        UserIdValidator validator = new(id);
        if (!validator.isValid)
        {
            ValidationException exception = new("ID da pessoa inválido");
            _logger.Error(exception, "ID da pessoa inválido");
            throw exception;
        }

        _logger.Information("Recuperando pessoa com ID[{Id}]", id);
        Pessoa? pessoa = await _pessoaRepository.GetPessoaById(id, true);
        if (pessoa is null)
        {
            PessoaNotFoundException pessoaNotFoundException = new(id);
            _logger.Error(pessoaNotFoundException, "Pessoa não encontrada");
            throw pessoaNotFoundException;
        }

        // Cascade: deletar todas as transações da pessoa
        if (pessoa.Transacoes.Any())
        {
            _logger.Information("Deletando {Count} transações da pessoa ID[{Id}]", 
                pessoa.Transacoes.Count, id);
            
            foreach (var transacao in pessoa.Transacoes.ToList())
            {
                await _transacaoRepository.DeleteTransacao(transacao.Id);
            }
        }

        _pessoaRepository.DeletePessoa(pessoa);
        await _pessoaRepository.FlushChanges();

        _logger.Information("Pessoa ID[{Id}] deletada com sucesso", id);
        
        return id;
    }
}