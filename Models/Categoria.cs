namespace HouseHoldeBudgetApi.Models;

// Models/Categoria.cs
/// <summary>
/// Representa uma categoria de transação
/// </summary>
public class Categoria
{
    /// <summary>
    /// Identificador único da categoria (auto-gerado)
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Descrição da categoria (ex: "Alimentação", "Salário")
    /// </summary>
    public string Descricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Finalidade da categoria (despesa, receita ou ambas)
    /// </summary>
    public FinalidadeCategoria Finalidade { get; set; }
    
    /// <summary>
    /// Lista de transações associadas a esta categoria (propriedade de navegação)
    /// </summary>
    public virtual ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
}