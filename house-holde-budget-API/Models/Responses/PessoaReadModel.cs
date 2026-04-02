namespace HouseHoldeBudgetApi.Models.Responses;

/// <summary>
/// Modelo de resposta para leitura de pessoa
/// </summary>
public class PessoaReadModel
{
    /// <summary>
    /// ID da pessoa
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Nome da pessoa
    /// </summary>
    public string Nome { get; set; } = string.Empty;
    
    /// <summary>
    /// Idade da pessoa
    /// </summary>
    public int Idade { get; set; }
    
    /// <summary>
    /// Total de transações associadas à pessoa
    /// </summary>
    public int TotalTransacoes { get; set; }
}