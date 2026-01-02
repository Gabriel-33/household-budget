// Controllers/IPessoaController.cs

using HouseholdBudgetApi.Models;
using HouseHoldeBudgetApi.Models;
using HouseHoldeBudgetApi.Models.Requests;
using HouseHoldeBudgetApi.Models.Responses;

namespace HouseholdBudgetApi.Controllers;

/// <summary>
/// Interface para controlador de pessoas no sistema de gastos.
/// </summary>
public interface IPessoaController
{
    /// <summary>
    /// Obtém todas as pessoas cadastradas com paginação.
    /// </summary>
    /// <param name="page">Número da página (começa em 1)</param>
    /// <param name="pageSize">Quantidade de itens por página</param>
    /// <returns>Lista paginada de pessoas</returns>
    Task<PessoasListResponse> GetPessoas(int page, int pageSize);
    
    /// <summary>
    /// Cria uma nova pessoa no sistema.
    /// </summary>
    /// <param name="request">Dados da pessoa a ser criada</param>
    /// <returns>Pessoa criada</returns>
    Task<PessoaReadModel> CreatePessoa(CreatePessoaRequestModel request);
    
    /// <summary>
    /// Deleta uma pessoa e todas as suas transações (cascade).
    /// </summary>
    /// <param name="id">ID da pessoa a ser deletada</param>
    /// <returns>ID da pessoa deletada</returns>
    Task<int> DeletePessoa(int id);
}