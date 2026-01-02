// Controllers/CategoriaController.cs
using FluentValidation;
using FluentValidation.Results;
using HouseholdBudgetApi.Models;
using HouseholdBudgetApi.Repositories;
using HouseHoldeBudgetApi.Mapper;
using HouseHoldeBudgetApi.Models.Requests;
using HouseHoldeBudgetApi.Models.Responses;
using ILogger = Serilog.ILogger;

namespace HouseholdBudgetApi.Controllers;

/// <summary>
/// Implementação do controlador de categorias no sistema de gastos.
/// </summary>
public class CategoriaController : ICategoriaController
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IValidator<CreateCategoriaRequestModel> _createCategoriaValidator;
    private readonly CategoriaModelMapper _categoriaModelMapper;
    private readonly ILogger _logger;

    public CategoriaController(
        ICategoriaRepository categoriaRepository,
        IValidator<CreateCategoriaRequestModel> createCategoriaValidator,
        CategoriaModelMapper categoriaModelMapper,
        ILogger logger)
    {
        _categoriaRepository = categoriaRepository;
        _createCategoriaValidator = createCategoriaValidator;
        _categoriaModelMapper = categoriaModelMapper;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todas as categorias cadastradas.
    /// </summary>
    public async Task<List<CategoriaReadModel>> GetCategorias()
    {
        _logger.Information("Recuperando todas as categorias cadastradas");
        
        var categorias = await _categoriaRepository.GetCategorias();
        
        var categoriasRead = categorias
            .Select(_categoriaModelMapper.CategoriaToCategoriaReadModel)
            .ToList();

        _logger.Information("Recuperado {Count} categorias", categoriasRead.Count);
        
        return categoriasRead;
    }

    /// <summary>
    /// Cria uma nova categoria.
    /// </summary>
    public async Task<CategoriaReadModel> CreateCategoria(CreateCategoriaRequestModel request)
    {
        _logger.Information("Validando requisição para criar categoria: Descricao[{Descricao}] Finalidade[{Finalidade}]",
            request.Descricao, request.Finalidade);

        ValidationResult validationResult = await _createCategoriaValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            ValidationException exception = new(validationResult.Errors.Select(e => e.ErrorMessage).ToString());
            _logger.Error(exception, "Requisição para criar categoria inválida");
            throw exception;
        }

        // Verificar se categoria com mesma descrição já existe
        _logger.Information("Verificando se categoria já existe: Descricao[{Descricao}]", request.Descricao);
        bool categoriaExists = await _categoriaRepository.CheckCategoriaExists(request.Descricao);
        if (categoriaExists)
        {
            ValidationException exception = new("Categoria com esta descrição já existe");
            _logger.Error(exception, "Categoria duplicada");
            throw exception;
        }

        _logger.Information("Criando nova categoria: Descricao[{Descricao}]", request.Descricao);
        
        var categoria = _categoriaModelMapper.CreateCategoriaRequestModelToCategoria(request);

        await _categoriaRepository.CreateCategoria(categoria);
        await _categoriaRepository.FlushChanges();

        _logger.Information("Categoria criada com ID[{Id}]", categoria.Id);

        return _categoriaModelMapper.CategoriaToCategoriaReadModel(categoria);
    }

    /// <summary>
    /// Obtém categorias por finalidade.
    /// </summary>
    public async Task<List<CategoriaReadModel>> GetCategoriasByFinalidade(FinalidadeCategoria finalidade)
    {
        _logger.Information("Recuperando categorias por finalidade: Finalidade[{Finalidade}]", finalidade);
        
        var categorias = await _categoriaRepository.GetCategoriasByFinalidade(finalidade);
        
        var categoriasRead = categorias
            .Select(_categoriaModelMapper.CategoriaToCategoriaReadModel)
            .ToList();

        _logger.Information("Recuperado {Count} categorias para finalidade {Finalidade}", 
            categoriasRead.Count, finalidade);
        
        return categoriasRead;
    }
}