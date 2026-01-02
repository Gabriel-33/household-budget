using HouseholdBudgetApi.Models;
using HouseHoldeBudgetApi.Models.Requests;
using HouseHoldeBudgetApi.Models.Responses;

namespace HouseHoldeBudgetApi.Mapper;

/// <summary>
/// Converte uma entidade Categoria para CategoriaReadModel
/// </summary>
/// <param name="categoria">Entidade Categoria</param>
/// <returns>CategoriaReadModel</returns>
public class CategoriaModelMapper
{
    public CategoriaReadModel CategoriaToCategoriaReadModel(Categoria categoria)
    {
        if (categoria == null)
            throw new ArgumentNullException(nameof(categoria));

        return new CategoriaReadModel
        {
            Id = categoria.Id,
            Descricao = categoria.Descricao,
            Finalidade = categoria.Finalidade
        };
    }

    /// <summary>
    /// Converte uma lista de entidades Categoria para lista de CategoriaReadModel
    /// </summary>
    /// <param name="categorias">Lista de entidades Categoria</param>
    /// <returns>Lista de CategoriaReadModel</returns>
    public List<CategoriaReadModel> CategoriasToCategoriaReadModels(IEnumerable<Categoria> categorias)
    {
        return categorias.Select(CategoriaToCategoriaReadModel).ToList();
    }

    /// <summary>
    /// Converte CreateCategoriaRequestModel para entidade Categoria
    /// </summary>
    /// <param name="request">Modelo de requisição</param>
    /// <returns>Entidade Categoria</returns>
    public Categoria CreateCategoriaRequestModelToCategoria(CreateCategoriaRequestModel request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        return new Categoria
        {
            Descricao = request.Descricao.Trim(),
            Finalidade = request.Finalidade
        };
    }

    /// <summary>
    /// Converte Categoria para CategoriaTotal (usado em relatórios)
    /// </summary>
    /// <param name="categoria">Entidade Categoria</param>
    /// <param name="totalReceitas">Total de receitas</param>
    /// <param name="totalDespesas">Total de despesas</param>
    /// <returns>CategoriaTotal</returns>
    public CategoriaTotal CategoriaToCategoriaTotal(Categoria categoria, decimal totalReceitas, decimal totalDespesas)
    {
        if (categoria == null)
            throw new ArgumentNullException(nameof(categoria));

        return new CategoriaTotal
        {
            CategoriaId = categoria.Id,
            Descricao = categoria.Descricao,
            Finalidade = categoria.Finalidade,
            TotalReceitas = totalReceitas,
            TotalDespesas = totalDespesas
        };
    }
}