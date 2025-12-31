// Models/Responses/TransacoesListResponse.cs

using HouseHoldeBudgetApi.Models.Responses;

namespace HouseholdBudgetApi.Models;

/// <summary>
/// Modelo de resposta para lista paginada de transações
/// </summary>
public class TransacoesListResponse
{
    public int maxPage { get; set; }
    public int totalCount { get; set; }
    public int pageCount { get; set; }
    public IReadOnlyList<TransacaoReadModel> transacoes { get; set; } = new List<TransacaoReadModel>();
}