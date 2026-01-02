// Controllers/TransacaoController.cs

using FluentValidation;
using FluentValidation.Results;
using HouseholdBudgetApi.Models;
using HouseholdBudgetApi.Repositories;
using HouseHoldeBudgetApi.Exceptions;
using HouseHoldeBudgetApi.Mapper;
using HouseHoldeBudgetApi.Models;
using HouseHoldeBudgetApi.Models.Requests;
using HouseHoldeBudgetApi.Models.Responses;
using HouseHoldeBudgetApi.Validators.CustomValidators.RequestQuery;
using ILogger = Serilog.ILogger;

namespace HouseholdBudgetApi.Controllers;

/// <summary>
/// Implementação do controlador de transações no sistema de gastos.
/// </summary>
public class TransacaoController : ITransacaoController
{
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly IPessoaRepository _pessoaRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IValidator<CreateTransacaoRequestModel> _createTransacaoValidator;
    private readonly TransacaoModelMapper _transacaoModelMapper;
    private readonly ILogger _logger;

    public TransacaoController(
        ITransacaoRepository transacaoRepository,
        IPessoaRepository pessoaRepository,
        ICategoriaRepository categoriaRepository,
        IValidator<CreateTransacaoRequestModel> createTransacaoValidator,
        TransacaoModelMapper transacaoModelMapper,
        ILogger logger)
    {
        _transacaoRepository = transacaoRepository;
        _pessoaRepository = pessoaRepository;
        _categoriaRepository = categoriaRepository;
        _createTransacaoValidator = createTransacaoValidator;
        _transacaoModelMapper = transacaoModelMapper;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todas as transações com paginação e filtros.
    /// </summary>
    public async Task<TransacoesListResponse> GetTransacoes(int page, int pageSize, int? pessoaId = null, string? tipo = null)
    {
        _logger.Information("Validando parâmetros de paginação: Page[{Page}] PageSize[{PageSize}] PessoaId[{PessoaId}] Tipo[{Tipo}]",
            page, pageSize, pessoaId, tipo);
    
        PageValidator validator = new(page, pageSize);
        if (!validator.isValid)
        {
            ValidationException exception = new("Parâmetros de paginação inválidos");
            _logger.Error("Parâmetros de paginação inválidos");
            throw exception;
        }

        // Validar tipo se fornecido
        if (!string.IsNullOrEmpty(tipo) && tipo != "Despesa" && tipo != "Receita")
        {
            ValidationException exception = new("Tipo inválido. Use 'Despesa' ou 'Receita'");
            _logger.Error(exception, "Tipo de transação inválido");
            throw exception;
        }

        _logger.Information("Recuperando transações: Page[{Page}] PageSize[{PageSize}]", page, pageSize);

        (var transacoes, int resultCount, int totalCount) = await _transacaoRepository
            .GetTransacoesAndCount(page, pageSize, pessoaId, tipo);

        var transacoesRead = transacoes
            .Select(_transacaoModelMapper.TransacaoToTransacaoReadModel)
            .ToList();

        _logger.Information("Recuperado {Count} transações da página Page[{Page}] PageSize[{PageSize}]",
            transacoesRead.Count, page, pageSize);

        int maxPage = totalCount / pageSize;
        if (totalCount % pageSize != 0)
            maxPage++;

        return new TransacoesListResponse
        {
            maxPage = maxPage,
            totalCount = totalCount,
            pageCount = resultCount,
            transacoes = transacoesRead
        };
    }
    /// <summary>
    /// Cria uma nova transação com validações automáticas.
    /// </summary>
    public async Task<TransacaoReadModel> CreateTransacao(CreateTransacaoRequestModel request)
    {
        _logger.Information("Validando requisição para criar transação: Descricao[{Descricao}] Valor[{Valor}] Tipo[{Tipo}]",
            request.Descricao, request.Valor, request.Tipo);

        // Validação básica com FluentValidation
        ValidationResult validationResult = await _createTransacaoValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            ValidationException exception = new(validationResult.Errors.Select(e => e.ErrorMessage).ToString());
            _logger.Error("Requisição para criar transação inválida");
            throw exception;
        }

        // Validação 1: Pessoa existe
        _logger.Information("Verificando existência da pessoa: ID[{PessoaId}]", request.PessoaId);
        Pessoa? pessoa = await _pessoaRepository.GetPessoaById(request.PessoaId);
        if (pessoa is null)
        {
            PessoaNotFoundException pessoaNotFoundException = new(request.PessoaId);
            _logger.Error(pessoaNotFoundException, "Pessoa não encontrada");
            throw pessoaNotFoundException;
        }

        // Validação 2: Categoria existe
        _logger.Information("Verificando existência da categoria: ID[{CategoriaId}]", request.CategoriaId);
        Categoria? categoria = await _categoriaRepository.GetCategoriaById(request.CategoriaId);
        if (categoria is null)
        {
            CategoriaNotFoundException categoriaNotFoundException = new(request.CategoriaId);
            _logger.Error(categoriaNotFoundException, "Categoria não encontrada");
            throw categoriaNotFoundException;
        }

        // Validação 3: Menor de idade só pode registrar despesas
        if (pessoa.Idade < 18 && request.Tipo != TipoTransacao.Despesa)
        {
            ValidationException exception = new("Menores de idade só podem registrar despesas");
            _logger.Error("Validação de idade falhou: Pessoa menor de idade tentando registrar receita");
            throw exception;
        }

        // Validação 4: Compatibilidade categoria/tipo
        if (!IsCategoriaCompativelComTipo(categoria.Finalidade, request.Tipo))
        {
            ValidationException exception = new(
                $"Categoria '{categoria.Descricao}' ({categoria.Finalidade}) não pode ser usada para {request.Tipo}"
            );
            _logger.Error("Incompatibilidade entre categoria e tipo de transação");
            throw exception;
        }

        // Validação 5: Valor deve ser positivo
        if (request.Valor <= 0)
        {
            ValidationException exception = new("Valor deve ser maior que zero");
            _logger.Error("Valor de transação inválido");
            throw exception;
        }

        _logger.Information("Criando nova transação para pessoa: Pessoa[{PessoaNome}]", pessoa.Nome);
        
        var transacao = _transacaoModelMapper.CreateTransacaoRequestModelToTransacaoWithEntities(
            request, pessoa, categoria);

        await _transacaoRepository.CreateTransacao(transacao);
        await _transacaoRepository.FlushChanges();

        _logger.Information("Transação criada com ID[{Id}] para pessoa ID[{PessoaId}]", 
            transacao.Id, transacao.PessoaId);

        return _transacaoModelMapper.TransacaoToTransacaoReadModel(transacao);
    }

    /// <summary>
    /// Verifica se uma categoria é compatível com o tipo de transação.
    /// </summary>
    private bool IsCategoriaCompativelComTipo(FinalidadeCategoria finalidade, TipoTransacao tipo)
    {
        return finalidade switch
        {
            FinalidadeCategoria.Despesa => tipo == TipoTransacao.Despesa,
            FinalidadeCategoria.Receita => tipo == TipoTransacao.Receita,
            FinalidadeCategoria.Ambas => true,
            _ => false
        };
    }
}