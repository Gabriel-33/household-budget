using System.ComponentModel;

namespace HouseholdBudgetApi.Models;

/// <summary>
/// Enum que define a finalidade de uma categoria
/// </summary>
public enum FinalidadeCategoria
{
    /// <summary>
    /// Categoria apenas para despesas
    /// </summary>
    [Description("Despesa")]
    Despesa,
    
    /// <summary>
    /// Categoria apenas para receitas
    /// </summary>
    [Description("Receita")]
    Receita,
    
    /// <summary>
    /// Categoria para despesas e receitas
    /// </summary>
    [Description("Ambas")]
    Ambas
}