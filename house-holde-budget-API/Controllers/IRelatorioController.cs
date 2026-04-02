// Controllers/IRelatorioController.cs
using HouseHoldeBudgetApi.Models.Responses;

namespace HouseholdBudgetApi.Controllers;

/// <summary>
/// Interface para controlador de relatórios no sistema de gastos.
/// </summary>
public interface IRelatorioController
{
    /// <summary>
    /// Gera relatório de totais por pessoa.
    /// </summary>
    /// <returns>Relatório com totais de receitas, despesas e saldo por pessoa</returns>
    Task<RelatorioPessoasResponse> GetTotaisPorPessoa();
    
    /// <summary>
    /// Gera relatório de totais por categoria (opcional).
    /// </summary>
    /// <returns>Relatório com totais de receitas, despesas e saldo por categoria</returns>
    Task<RelatorioCategoriasResponse> GetTotaisPorCategoria();
}