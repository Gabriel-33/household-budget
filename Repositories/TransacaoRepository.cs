// Repositories/TransacaoRepository.cs
using Microsoft.EntityFrameworkCore;
using HouseHoldeBudgetApi.Context;
using HouseHoldeBudgetApi.Models;

namespace HouseholdBudgetApi.Repositories;

/// <summary>
/// Implementação do repositório de transações no sistema de gastos.
/// </summary>
public class TransacaoRepository : ITransacaoRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public TransacaoRepository(AppDbContext dbContext, IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContext = dbContext;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Transacao?> GetTransacaoById(int id)
    {
        return await _dbContext.Transacoes
            .AsNoTracking()
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public Task<IList<Transacao>> GetTransacoes(int page, int pageSize, int? pessoaId = null, string? tipo = null) =>
        GetTransacoes(_dbContext, page, pageSize, pessoaId, tipo);

    private async Task<IList<Transacao>> GetTransacoesWFactory(int page, int pageSize, int? pessoaId = null, string? tipo = null)
    {
        await using AppDbContext? inDbContext = await _dbContextFactory.CreateDbContextAsync();

        if (inDbContext is null)
            throw new("Não foi possível instanciar um novo DbContext");

        return await GetTransacoes(inDbContext, page, pageSize, pessoaId, tipo);
    }

    private async Task<IList<Transacao>> GetTransacoes(AppDbContext inDbContext, int page, int pageSize, int? pessoaId = null, string? tipo = null)
    {
        var query = inDbContext.Transacoes
            .AsNoTracking()
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .OrderByDescending(t => t.Data)
            .ThenByDescending(t => t.Id)
            .AsQueryable();

        // Aplicar filtros
        if (pessoaId.HasValue)
            query = query.Where(t => t.PessoaId == pessoaId.Value);

        if (!string.IsNullOrEmpty(tipo) && Enum.TryParse<TipoTransacao>(tipo, out var tipoEnum))
            query = query.Where(t => t.Tipo == tipoEnum);

        var result = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return result;
    }

    public Task<int> GetTransacoesCount(int? pessoaId = null, string? tipo = null) =>
        GetTransacoesCount(_dbContext, pessoaId, tipo);

    private async Task<int> GetTransacoesCountWFactory(int? pessoaId = null, string? tipo = null)
    {
        await using AppDbContext? inDbContext = await _dbContextFactory.CreateDbContextAsync();

        if (inDbContext is null)
            throw new("Não foi possível instanciar um novo DbContext");

        return await GetTransacoesCount(inDbContext, pessoaId, tipo);
    }

    private async Task<int> GetTransacoesCount(AppDbContext inDbContext, int? pessoaId = null, string? tipo = null)
    {
        var query = inDbContext.Transacoes.AsQueryable();

        if (pessoaId.HasValue)
            query = query.Where(t => t.PessoaId == pessoaId.Value);

        if (!string.IsNullOrEmpty(tipo) && Enum.TryParse<TipoTransacao>(tipo, out var tipoEnum))
            query = query.Where(t => t.Tipo == tipoEnum);

        return await query.CountAsync();
    }

    public async Task<(IList<Transacao>, int, int)> GetTransacoesAndCount(int page, int pageSize, int? pessoaId = null, string? tipo = null)
    {
        var transacoesTask = GetTransacoesWFactory(page, pageSize, pessoaId, tipo);
        var transacoesCountTask = GetTransacoesCountWFactory(pessoaId, tipo);
        await Task.WhenAll(transacoesTask, transacoesCountTask);

        var result = transacoesTask.Result;
        int totalCount = transacoesCountTask.Result;
        return (result, result.Count, totalCount);
    }

    public async Task<List<Transacao>> GetAllWithDetailsAsync()
    {
        return await _dbContext.Transacoes
            .AsNoTracking()
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .OrderByDescending(t => t.Data)
            .ToListAsync();
    }

    public async Task<IList<Transacao>> GetTransacoesByPessoaId(int pessoaId)
    {
        return await _dbContext.Transacoes
            .AsNoTracking()
            .Include(t => t.Categoria)
            .Where(t => t.PessoaId == pessoaId)
            .OrderByDescending(t => t.Data)
            .ToListAsync();
    }

    public async Task<int> GetTransacoesCountByPessoaId(int pessoaId)
    {
        return await _dbContext.Transacoes
            .CountAsync(t => t.PessoaId == pessoaId);
    }

    public async Task CreateTransacao(Transacao transacao)
    {
        await _dbContext.Transacoes.AddAsync(transacao);
    }

    public async Task DeleteTransacao(int id)
    {
        var transacao = await _dbContext.Transacoes.FindAsync(id);
        if (transacao != null)
        {
            _dbContext.Transacoes.Remove(transacao);
        }
    }

    public async Task DeleteTransacoesByPessoaId(int pessoaId)
    {
        var transacoes = await _dbContext.Transacoes
            .Where(t => t.PessoaId == pessoaId)
            .ToListAsync();

        if (transacoes.Any())
        {
            _dbContext.Transacoes.RemoveRange(transacoes);
        }
    }

    public async Task FlushChanges()
    {
        await _dbContext.SaveChangesAsync();
    }
}