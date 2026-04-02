// Models/Responses/GastosListResponse.cs

using HouseHoldeBudgetApi.Models.Responses;

namespace HouseholdBudgetApi.Models;

/// <summary>
/// Modelo de resposta para listas paginadas do sistema de gastos
/// </summary>
public class GastosListResponse<T>
{
    /// <summary>
    /// Número máximo de páginas disponíveis
    /// </summary>
    public int maxPage { get; set; }
    
    /// <summary>
    /// Total de itens no banco de dados
    /// </summary>
    public int totalCount { get; set; }
    
    /// <summary>
    /// Quantidade de itens na página atual
    /// </summary>
    public int pageCount { get; set; }
    
    /// <summary>
    /// Lista de itens da página atual
    /// </summary>
    public IReadOnlyList<T> items { get; set; } = new List<T>();
}

// Especializações para cada tipo
public class PessoasListResponse : GastosListResponse<PessoaReadModel> { }