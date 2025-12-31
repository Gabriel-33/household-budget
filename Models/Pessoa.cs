namespace HouseHoldeBudgetApi.Models;

/// <summary>
/// Representa uma pessoa no sistema de gastos
/// </summary>
public class Pessoa
{
    /// <summary>
    /// Identificador único da pessoa (auto-gerado)
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Nome completo da pessoa
    /// </summary>
    public string Nome { get; set; } = string.Empty;
    
    /// <summary>
    /// Idade da pessoa (número inteiro positivo)
    /// </summary>
    public int Idade { get; set; }
    
    /// <summary>
    /// Data de criação do registro
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Data da última atualização do registro
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Lista de transações associadas à pessoa
    /// </summary>
    public virtual ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
}