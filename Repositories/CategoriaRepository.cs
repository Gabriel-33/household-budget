// Repositories/CategoriaRepository.cs
using Microsoft.EntityFrameworkCore;
using HouseHoldeBudgetApi.Context;
using HouseHoldeBudgetApi.Models;

namespace HouseholdBudgetApi.Repositories;

/// <summary>
/// Implementação do repositório de categorias no sistema de gastos.
/// </summary>
public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public CategoriaRepository(AppDbContext dbContext, IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContext = dbContext;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Categoria?> GetCategoriaById(int id)
    {
        return await _dbContext.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Categoria?> GetCategoriaByDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            return null;

        return await _dbContext.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Descricao.ToLower() == descricao.Trim().ToLower());
    }

    public async Task<Categoria?> GetCategoriaByIdWithTransacoes(int id)
    {
        return await _dbContext.Categorias
            .AsNoTracking()
            .Include(c => c.Transacoes)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public Task<IList<Categoria>> GetCategorias() => GetCategorias(_dbContext);

    private async Task<IList<Categoria>> GetCategoriasWFactory()
    {
        await using AppDbContext? inDbContext = await _dbContextFactory.CreateDbContextAsync();

        if (inDbContext is null)
            throw new("Não foi possível instanciar um novo DbContext");

        return await GetCategorias(inDbContext);
    }

    private async Task<IList<Categoria>> GetCategorias(AppDbContext inDbContext)
    {
        return await inDbContext.Categorias
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .ToListAsync();
    }

    public async Task<IList<Categoria>> GetCategoriasByFinalidade(FinalidadeCategoria finalidade)
    {
        return await _dbContext.Categorias
            .AsNoTracking()
            .Where(c => c.Finalidade == finalidade || c.Finalidade == FinalidadeCategoria.Ambas)
            .OrderBy(c => c.Descricao)
            .ToListAsync();
    }

    public async Task<bool> CheckCategoriaExists(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            return false;

        return await _dbContext.Categorias
            .AnyAsync(c => c.Descricao.ToLower() == descricao.Trim().ToLower());
    }

    public async Task<bool> CheckCategoriaExists(int id)
    {
        return await _dbContext.Categorias
            .AnyAsync(c => c.Id == id);
    }

    public async Task CreateCategoria(Categoria categoria)
    {
        await _dbContext.Categorias.AddAsync(categoria);
    }

    public async Task FlushChanges()
    {
        await _dbContext.SaveChangesAsync();
    }
}