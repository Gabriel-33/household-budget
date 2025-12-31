// Repositories/IPessoaRepository.cs
using HouseHoldeBudgetApi.Models;

namespace HouseholdBudgetApi.Repositories;

/// <summary>
/// Interface para repositório de pessoas no sistema de gastos.
/// </summary>
public interface IPessoaRepository
{
    /// <summary>
    /// Obtém uma pessoa pelo ID.
    /// </summary>
    /// <param name="id">ID da pessoa</param>
    /// <param name="onlyFindAsync">Se true, usa FindAsync; se false, inclui transações</param>
    /// <returns>Pessoa encontrada ou null</returns>
    Task<Pessoa?> GetPessoaById(int id, bool onlyFindAsync = false);
    
    /// <summary>
    /// Obtém uma pessoa pelo nome.
    /// </summary>
    /// <param name="nome">Nome da pessoa</param>
    /// <returns>Pessoa encontrada ou null</returns>
    Task<Pessoa?> GetPessoaByNome(string nome);
    
    /// <summary>
    /// Obtém todas as pessoas com paginação.
    /// </summary>
    /// <param name="page">Número da página</param>
    /// <param name="pageSize">Itens por página</param>
    /// <returns>Lista de pessoas</returns>
    Task<IList<Pessoa>> GetPessoas(int page, int pageSize);
    
    /// <summary>
    /// Obtém o total de pessoas cadastradas.
    /// </summary>
    /// <returns>Total de pessoas</returns>
    Task<int> GetPessoasCount();
    
    /// <summary>
    /// Obtém pessoas com contagem total (para paginação).
    /// </summary>
    /// <param name="page">Número da página</param>
    /// <param name="pageSize">Itens por página</param>
    /// <returns>Tupla com lista, contagem da página e total</returns>
    Task<(IList<Pessoa>, int, int)> GetPessoasAndCount(int page, int pageSize);
    
    /// <summary>
    /// Obtém todas as pessoas com suas transações incluídas.
    /// </summary>
    /// <returns>Lista de pessoas com transações</returns>
    Task<List<Pessoa>> GetAllWithTransacoesAsync();
    
    /// <summary>
    /// Cria uma nova pessoa.
    /// </summary>
    /// <param name="pessoa">Pessoa a ser criada</param>
    Task CreatePessoa(Pessoa pessoa);
    
    /// <summary>
    /// Deleta uma pessoa.
    /// </summary>
    /// <param name="pessoa">Pessoa a ser deletada</param>
    void DeletePessoa(Pessoa pessoa);
    
    /// <summary>
    /// Salva alterações no banco de dados.
    /// </summary>
    Task FlushChanges();
}