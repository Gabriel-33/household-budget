namespace HouseHoldeBudgetApi.Models;

// Models/Transacao.cs
/// <summary>
/// Representa uma transação financeira
/// </summary>
public class Transacao
{
    /// <summary>
    /// Identificador único da transação (auto-gerado)
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Descrição da transação
    /// </summary>
    public string Descricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Valor da transação (número decimal positivo)
    /// </summary>
    public decimal Valor { get; set; }
    
    /// <summary>
    /// Tipo da transação (despesa ou receita)
    /// </summary>
    public TipoTransacao Tipo { get; set; }
    
    /// <summary>
    /// Data da transação
    /// </summary>
    public DateTime Data { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// ID da categoria da transação (chave estrangeira)
    /// </summary>
    public int CategoriaId { get; set; }
    
    /// <summary>
    /// ID da pessoa associada à transação (chave estrangeira)
    /// </summary>
    public int PessoaId { get; set; }
    
    /// <summary>
    /// Categoria da transação (propriedade de navegação)
    /// </summary>
    public virtual Categoria Categoria { get; set; } = null!;
    
    /// <summary>
    /// Pessoa associada à transação (propriedade de navegação)
    /// </summary>
    public virtual Pessoa Pessoa { get; set; } = null!;
}
