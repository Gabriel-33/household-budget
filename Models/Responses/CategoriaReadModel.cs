namespace HouseHoldeBudgetApi.Models.Responses;

/// <summary>
/// Modelo de resposta para leitura de categoria
/// </summary>
public class CategoriaReadModel
{
    /// <summary>
    /// ID da categoria
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Descrição da categoria
    /// </summary>
    public string Descricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Finalidade da categoria
    /// </summary>
    public FinalidadeCategoria Finalidade { get; set; }
}