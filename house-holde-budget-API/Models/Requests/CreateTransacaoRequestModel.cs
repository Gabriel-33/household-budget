namespace HouseHoldeBudgetApi.Models.Requests;

/// <summary>
/// Modelo de requisição para criação de uma nova transação
/// </summary>
public class CreateTransacaoRequestModel
{
    /// <summary>
    /// Descrição da transação (obrigatório, máximo 200 caracteres)
    /// </summary>
    public string Descricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Valor da transação (obrigatório, deve ser positivo)
    /// </summary>
    public decimal Valor { get; set; }
    
    /// <summary>
    /// Tipo da transação (despesa ou receita)
    /// </summary>
    public TipoTransacao Tipo { get; set; }
    
    /// <summary>
    /// ID da categoria da transação (obrigatório)
    /// </summary>
    public int CategoriaId { get; set; }
    
    /// <summary>
    /// ID da pessoa associada à transação (obrigatório)
    /// </summary>
    public int PessoaId { get; set; }
}