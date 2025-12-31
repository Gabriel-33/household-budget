// Repositories/ITransacaoRepository.cs
using HouseHoldeBudgetApi.Models;

namespace HouseholdBudgetApi.Repositories;

/// <summary>
/// Interface para repositório de transações no sistema de gastos.
/// </summary>
public interface ITransacaoRepository
{
    /// <summary>
    /// Obtém uma transação pelo ID.
    /// </summary>
    Task<Transacao?> GetTransacaoById(int id);
    
    /// <summary>
    /// Obtém transações com paginação e filtros opcionais.
    /// </summary>
    Task<IList<Transacao>> GetTransacoes(int page, int pageSize, int? pessoaId = null, string? tipo = null);
    
    /// <summary>
    /// Obtém o total de transações com filtros opcionais.
    /// </summary>
    Task<int> GetTransacoesCount(int? pessoaId = null, string? tipo = null);
    
    /// <summary>
    /// Obtém transações com contagem total (para paginação).
    /// </summary>
    Task<(IList<Transacao>, int, int)> GetTransacoesAndCount(int page, int pageSize, int? pessoaId = null, string? tipo = null);
    
    /// <summary>
    /// Obtém todas as transações com detalhes (include Pessoa e Categoria).
    /// </summary>
    Task<List<Transacao>> GetAllWithDetailsAsync();
    
    /// <summary>
    /// Obtém transações por ID da pessoa.
    /// </summary>
    Task<IList<Transacao>> GetTransacoesByPessoaId(int pessoaId);
    
    /// <summary>
    /// Obtém total de transações por pessoa.
    /// </summary>
    Task<int> GetTransacoesCountByPessoaId(int pessoaId);
    
    /// <summary>
    /// Cria uma nova transação.
    /// </summary>
    Task CreateTransacao(Transacao transacao);
    
    /// <summary>
    /// Deleta uma transação pelo ID.
    /// </summary>
    Task DeleteTransacao(int id);
    
    /// <summary>
    /// Deleta todas as transações de uma pessoa.
    /// </summary>
    Task DeleteTransacoesByPessoaId(int pessoaId);
    
    /// <summary>
    /// Salva alterações no banco de dados.
    /// </summary>
    Task FlushChanges();
}