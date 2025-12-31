namespace HouseHoldeBudgetApi.Models.Requests;

/// <summary>
/// Modelo de requisição para criação de uma nova categoria
/// </summary>
public class CreateCategoriaRequestModel
{
    /// <summary>
    /// Descrição da categoria (obrigatório, máximo 50 caracteres)
    /// </summary>
    public string Descricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Finalidade da categoria (despesa, receita ou ambas)
    /// </summary>
    public FinalidadeCategoria Finalidade { get; set; }
}