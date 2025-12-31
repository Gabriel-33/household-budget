// Controllers/RelatorioController.cs
using HouseholdBudgetApi.Repositories;
using HouseHoldeBudgetApi.Mapper;
using HouseHoldeBudgetApi.Models.Responses;
using ILogger = Serilog.ILogger;

namespace HouseholdBudgetApi.Controllers;

/// <summary>
/// Implementação do controlador de relatórios no sistema de gastos.
/// </summary>
public class RelatorioController : IRelatorioController
{
    private readonly IRelatorioRepository _relatorioRepository;
    private readonly PessoaModelMapper _pessoaModelMapper;
    private readonly CategoriaModelMapper _categoriaModelMapper;
    private readonly ILogger _logger;

    public RelatorioController(
        IRelatorioRepository relatorioRepository,
        PessoaModelMapper pessoaModelMapper,
        CategoriaModelMapper categoriaModelMapper,
        ILogger logger)
    {
        _relatorioRepository = relatorioRepository;
        _pessoaModelMapper = pessoaModelMapper;
        _categoriaModelMapper = categoriaModelMapper;
        _logger = logger;
    }

    /// <summary>
    /// Gera relatório de totais por pessoa.
    /// </summary>
    public async Task<RelatorioPessoasResponse> GetTotaisPorPessoa()
    {
        _logger.Information("Gerando relatório de totais por pessoa");
        
        // Usar o repository para obter os dados
        var pessoasTotais = await _relatorioRepository.GetTotaisPorPessoaAsync();
        var totaisGerais = await _relatorioRepository.GetTotaisGeraisAsync();
        
        // Ordenar por maior saldo (ou você pode ordenar por nome, etc)
        pessoasTotais = pessoasTotais
            .OrderByDescending(p => p.Saldo)
            .ThenBy(p => p.Nome)
            .ToList();

        _logger.Information("Relatório gerado para {Count} pessoas", pessoasTotais.Count);
        
        return new RelatorioPessoasResponse
        {
            Pessoas = pessoasTotais,
            TotalGeral = totaisGerais
        };
    }

    /// <summary>
    /// Gera relatório de totais por categoria (opcional).
    /// </summary>
    public async Task<RelatorioCategoriasResponse> GetTotaisPorCategoria()
    {
        _logger.Information("Gerando relatório de totais por categoria");
        
        // Usar o repository para obter os dados
        var categoriasTotais = await _relatorioRepository.GetTotaisPorCategoriaAsync();
        var totaisGerais = await _relatorioRepository.GetTotaisGeraisAsync();
        
        // Ordenar por maior valor total (absoluto)
        categoriasTotais = categoriasTotais
            .OrderByDescending(c => c.TotalReceitas + c.TotalDespesas)
            .ThenBy(c => c.Descricao)
            .ToList();

        _logger.Information("Relatório gerado para {Count} categorias", categoriasTotais.Count);
        
        return new RelatorioCategoriasResponse
        {
            Categorias = categoriasTotais,
            TotalGeral = totaisGerais
        };
    }
}