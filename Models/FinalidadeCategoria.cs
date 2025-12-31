namespace HouseHoldeBudgetApi.Models;

/// <summary>
/// Enum que define a finalidade de uma categoria
/// </summary>
public enum FinalidadeCategoria
{
    /// <summary>
    /// Categoria apenas para despesas
    /// </summary>
    Despesa,
    
    /// <summary>
    /// Categoria apenas para receitas
    /// </summary>
    Receita,
    
    /// <summary>
    /// Categoria para despesas e receitas
    /// </summary>
    Ambas
}