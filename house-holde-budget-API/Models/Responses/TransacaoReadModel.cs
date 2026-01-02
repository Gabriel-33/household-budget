namespace HouseHoldeBudgetApi.Models.Responses;

/// <summary>
/// Modelo de resposta para leitura de transação
/// </summary>
public class TransacaoReadModel
{
    /// <summary>
    /// ID da transação
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Descrição da transação
    /// </summary>
    public string Descricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Valor da transação
    /// </summary>
    public decimal Valor { get; set; }
    
    /// <summary>
    /// Tipo da transação
    /// </summary>
    public TipoTransacao Tipo { get; set; }
    
    /// <summary>
    /// Data da transação
    /// </summary>
    public DateTime Data { get; set; }
    
    /// <summary>
    /// Descrição da categoria
    /// </summary>
    public string CategoriaDescricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Nome da pessoa associada
    /// </summary>
    public string PessoaNome { get; set; } = string.Empty;
}