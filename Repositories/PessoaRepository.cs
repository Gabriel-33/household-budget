// Repositories/PessoaRepository.cs
using Microsoft.EntityFrameworkCore;
using HouseHoldeBudgetApi.Context;
using HouseHoldeBudgetApi.Models;

namespace HouseholdBudgetApi.Repositories;

/// <summary>
/// Implementação do repositório de pessoas no sistema de gastos.
/// </summary>
public class PessoaRepository : IPessoaRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public PessoaRepository(AppDbContext dbContext, IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContext = dbContext;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Pessoa?> GetPessoaById(int id, bool onlyFindAsync = false)
    {
        Pessoa? pessoa = null;

        if (onlyFindAsync)
            pessoa = await _dbContext.Pessoas.FindAsync(id);
        else
            pessoa = await _dbContext.Pessoas
                .AsNoTracking()
                .Include(p => p.Transacoes)
                .FirstOrDefaultAsync(p => p.Id == id);

        return pessoa;
    }

    public async Task<Pessoa?> GetPessoaByNome(string nome)
    {
        return await _dbContext.Pessoas
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Nome == nome);
    }

    public Task<IList<Pessoa>> GetPessoas(int page, int pageSize) =>
        GetPessoas(_dbContext, page, pageSize);

    private async Task<IList<Pessoa>> GetPessoasWFactory(int page, int pageSize)
    {
        await using AppDbContext? inDbContext = await _dbContextFactory.CreateDbContextAsync();

        if (inDbContext is null)
            throw new("Não foi possível instanciar um novo DbContext");

        return await GetPessoas(inDbContext, page, pageSize);
    }

    private async Task<IList<Pessoa>> GetPessoas(AppDbContext inDbContext, int page, int pageSize)
    {
        var result = await inDbContext.Pessoas
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return result;
    }

    public Task<int> GetPessoasCount() => GetPessoasCount(_dbContext);

    private async Task<int> GetPessoasCountWFactory()
    {
        await using AppDbContext? inDbContext = await _dbContextFactory.CreateDbContextAsync();

        if (inDbContext is null)
            throw new("Não foi possível instanciar um novo DbContext");

        return await GetPessoasCount(inDbContext);
    }

    private async Task<int> GetPessoasCount(AppDbContext inDbContext)
    {
        return await inDbContext.Pessoas.CountAsync();
    }

    public async Task<(IList<Pessoa>, int, int)> GetPessoasAndCount(int page, int pageSize)
    {
        var pessoasTask = GetPessoasWFactory(page, pageSize);
        var pessoasCountTask = GetPessoasCountWFactory();
        await Task.WhenAll(pessoasTask, pessoasCountTask);

        var result = pessoasTask.Result;
        int totalCount = pessoasCountTask.Result;
        return (result, result.Count, totalCount);
    }

    public async Task<List<Pessoa>> GetAllWithTransacoesAsync()
    {
        return await _dbContext.Pessoas
            .AsNoTracking()
            .Include(p => p.Transacoes)
            .ThenInclude(t => t.Categoria)
            .ToListAsync();
    }

    public async Task CreatePessoa(Pessoa pessoa)
    {
        await _dbContext.Pessoas.AddAsync(pessoa);
    }

    public void DeletePessoa(Pessoa pessoa)
    {
        _dbContext.Pessoas.Remove(pessoa);
    }

    public async Task FlushChanges()
    {
        await _dbContext.SaveChangesAsync();
    }
}