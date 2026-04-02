// Repositories/RelatorioRepository.cs
using Microsoft.EntityFrameworkCore;
using HouseHoldeBudgetApi.Context;
using HouseHoldeBudgetApi.Models;
using HouseHoldeBudgetApi.Models.Responses;

namespace HouseholdBudgetApi.Repositories;

/// <summary>
/// Implementação do repositório de relatórios (opcional).
/// </summary>
public class RelatorioRepository : IRelatorioRepository
{
    private readonly AppDbContext _dbContext;

    public RelatorioRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<PessoaTotal>> GetTotaisPorPessoaAsync()
    {
        var query = from pessoa in _dbContext.Pessoas
                    join transacao in _dbContext.Transacoes on pessoa.Id equals transacao.PessoaId into transacoes
                    from t in transacoes.DefaultIfEmpty()
                    group t by new { pessoa.Id, pessoa.Nome, pessoa.Idade } into g
                    select new PessoaTotal
                    {
                        PessoaId = g.Key.Id,
                        Nome = g.Key.Nome,
                        Idade = g.Key.Idade,
                        TotalReceitas = g.Where(t => t != null && t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor),
                        TotalDespesas = g.Where(t => t != null && t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor)
                    };

        return await query.ToListAsync();
    }

    public async Task<List<CategoriaTotal>> GetTotaisPorCategoriaAsync()
    {
        var query = from categoria in _dbContext.Categorias
                    join transacao in _dbContext.Transacoes on categoria.Id equals transacao.CategoriaId into transacoes
                    from t in transacoes.DefaultIfEmpty()
                    group t by new { categoria.Id, categoria.Descricao, categoria.Finalidade } into g
                    select new CategoriaTotal
                    {
                        CategoriaId = g.Key.Id,
                        Descricao = g.Key.Descricao,
                        Finalidade = g.Key.Finalidade,
                        TotalReceitas = g.Where(t => t != null && t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor),
                        TotalDespesas = g.Where(t => t != null && t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor)
                    };

        return await query.ToListAsync();
    }

    public async Task<TotalGeral> GetTotaisGeraisAsync()
    {
        var receitas = await _dbContext.Transacoes
            .Where(t => t.Tipo == TipoTransacao.Receita)
            .SumAsync(t => t.Valor);
        
        var despesas = await _dbContext.Transacoes
            .Where(t => t.Tipo == TipoTransacao.Despesa)
            .SumAsync(t => t.Valor);

        return new TotalGeral
        {
            TotalReceitas = receitas,
            TotalDespesas = despesas
        };
    }
}