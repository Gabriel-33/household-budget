namespace HouseHoldeBudgetApi.Models.Requests;

/// <summary>
/// Modelo de requisição para criação de uma nova pessoa
/// </summary>
public class CreatePessoaRequestModel
{
    /// <summary>
    /// Nome da pessoa (obrigatório, máximo 100 caracteres)
    /// </summary>
    public string Nome { get; set; } = string.Empty;
    
    /// <summary>
    /// Idade da pessoa (obrigatório, deve ser entre 0 e 150)
    /// </summary>
    public int Idade { get; set; }
}