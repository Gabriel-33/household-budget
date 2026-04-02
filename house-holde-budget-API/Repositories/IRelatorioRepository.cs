// Repositories/IRelatorioRepository.cs
using HouseHoldeBudgetApi.Models.Responses;

namespace HouseholdBudgetApi.Repositories;

/// <summary>
/// Interface para repositório de relatórios (opcional - pode fazer no controller).
/// </summary>
public interface IRelatorioRepository
{
    /// <summary>
    /// Obtém os totais agrupados por pessoa.
    /// </summary>
    Task<List<PessoaTotal>> GetTotaisPorPessoaAsync();
    
    /// <summary>
    /// Obtém os totais agrupados por categoria.
    /// </summary>
    Task<List<CategoriaTotal>> GetTotaisPorCategoriaAsync();
    
    /// <summary>
    /// Obtém os totais gerais do sistema.
    /// </summary>
    Task<TotalGeral> GetTotaisGeraisAsync();
}