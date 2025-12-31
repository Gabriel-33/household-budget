namespace HouseHoldeBudgetApi.Models;

/// <summary>
/// Enum que define o tipo de transação
/// </summary>
public enum TipoTransacao
{
    /// <summary>
    /// Transação de despesa (saída de dinheiro)
    /// </summary>
    Despesa,
    
    /// <summary>
    /// Transação de receita (entrada de dinheiro)
    /// </summary>
    Receita
}