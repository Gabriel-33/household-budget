// Controllers/ICategoriaController.cs
using HouseHoldeBudgetApi.Models;
using HouseHoldeBudgetApi.Models.Requests;
using HouseHoldeBudgetApi.Models.Responses;

namespace HouseholdBudgetApi.Controllers;

/// <summary>
/// Interface para controlador de categorias no sistema de gastos.
/// </summary>
public interface ICategoriaController
{
    /// <summary>
    /// Obtém todas as categorias cadastradas.
    /// </summary>
    /// <returns>Lista de categorias</returns>
    Task<List<CategoriaReadModel>> GetCategorias();
    
    /// <summary>
    /// Cria uma nova categoria.
    /// </summary>
    /// <param name="request">Dados da categoria a ser criada</param>
    /// <returns>Categoria criada</returns>
    Task<CategoriaReadModel> CreateCategoria(CreateCategoriaRequestModel request);
    
    /// <summary>
    /// Obtém categorias por finalidade.
    /// </summary>
    /// <param name="finalidade">Finalidade desejada</param>
    /// <returns>Lista de categorias com a finalidade especificada</returns>
    Task<List<CategoriaReadModel>> GetCategoriasByFinalidade(FinalidadeCategoria finalidade);
}