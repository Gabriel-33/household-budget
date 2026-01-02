using System.ComponentModel;

namespace HouseHoldeBudgetApi.Models;

/// <summary>
/// Enum que define o tipo de transação
/// </summary>

/// <summary>
/// Enum que define o tipo de transação
/// </summary>
public enum TipoTransacao
{
    /// <summary>
    /// Transação de despesa (saída de dinheiro)
    /// </summary>
    [Description("Despesa")]
    Despesa,
    
    /// <summary>
    /// Transação de receita (entrada de dinheiro)
    /// </summary>
    [Description("Receita")]
    Receita
}