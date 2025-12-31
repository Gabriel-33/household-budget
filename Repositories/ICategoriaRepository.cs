// Repositories/ICategoriaRepository.cs

using HouseHoldeBudgetApi.Models;

namespace HouseholdBudgetApi.Repositories;

/// <summary>
/// Interface para repositório de categorias no sistema de gastos.
/// </summary>
public interface ICategoriaRepository
{
    /// <summary>
    /// Obtém uma categoria pelo ID.
    /// </summary>
    /// <param name="id">ID da categoria</param>
    /// <returns>Categoria encontrada ou null</returns>
    Task<Categoria?> GetCategoriaById(int id);
    
    /// <summary>
    /// Obtém uma categoria pela descrição.
    /// </summary>
    /// <param name="descricao">Descrição da categoria</param>
    /// <returns>Categoria encontrada ou null</returns>
    Task<Categoria?> GetCategoriaByDescricao(string descricao);
    
    /// <summary>
    /// Obtém uma categoria pelo ID incluindo suas transações.
    /// </summary>
    /// <param name="id">ID da categoria</param>
    /// <returns>Categoria com transações ou null</returns>
    Task<Categoria?> GetCategoriaByIdWithTransacoes(int id);
    
    /// <summary>
    /// Obtém todas as categorias cadastradas.
    /// </summary>
    /// <returns>Lista de categorias</returns>
    Task<IList<Categoria>> GetCategorias();
    
    /// <summary>
    /// Obtém categorias por finalidade.
    /// </summary>
    /// <param name="finalidade">Finalidade da categoria</param>
    /// <returns>Lista de categorias com a finalidade especificada</returns>
    Task<IList<Categoria>> GetCategoriasByFinalidade(FinalidadeCategoria finalidade);
    
    /// <summary>
    /// Verifica se uma categoria com a descrição fornecida já existe.
    /// </summary>
    /// <param name="descricao">Descrição da categoria</param>
    /// <returns>True se a categoria existe, False caso contrário</returns>
    Task<bool> CheckCategoriaExists(string descricao);
    
    /// <summary>
    /// Verifica se uma categoria com o ID fornecido existe.
    /// </summary>
    /// <param name="id">ID da categoria</param>
    /// <returns>True se a categoria existe, False caso contrário</returns>
    Task<bool> CheckCategoriaExists(int id);
    
    /// <summary>
    /// Cria uma nova categoria.
    /// </summary>
    /// <param name="categoria">Categoria a ser criada</param>
    Task CreateCategoria(Categoria categoria);
    
    /// <summary>
    /// Salva alterações no banco de dados.
    /// </summary>
    Task FlushChanges();
}