// Controllers/ITransacaoController.cs

using HouseholdBudgetApi.Models;
using HouseHoldeBudgetApi.Models;
using HouseHoldeBudgetApi.Models.Requests;
using HouseHoldeBudgetApi.Models.Responses;

namespace HouseholdBudgetApi.Controllers;

/// <summary>
/// Interface para controlador de transações no sistema de gastos.
/// </summary>
public interface ITransacaoController
{
    /// <summary>
    /// Obtém todas as transações com paginação e filtros opcionais.
    /// </summary>
    /// <param name="page">Número da página (começa em 1)</param>
    /// <param name="pageSize">Quantidade de itens por página</param>
    /// <param name="pessoaId">Filtro opcional por ID da pessoa</param>
    /// <param name="tipo">Filtro opcional por tipo (Despesa/Receita)</param>
    /// <returns>Lista paginada de transações</returns>
    Task<TransacoesListResponse> GetTransacoes(int page, int pageSize, int? pessoaId = null, string? tipo = null);
    
    /// <summary>
    /// Cria uma nova transação com validações automáticas.
    /// </summary>
    /// <param name="request">Dados da transação a ser criada</param>
    /// <returns>Transação criada</returns>
    Task<TransacaoReadModel> CreateTransacao(CreateTransacaoRequestModel request);
}