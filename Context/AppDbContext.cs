using HouseholdBudgetApi.Models;
using Microsoft.EntityFrameworkCore;
using HouseHoldeBudgetApi.Models;
using HouseHoldeBudgetApi.Services;
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace HouseHoldeBudgetApi.Context;

/// <summary>
/// DbContext da aplicação, responsável por mapear as entidades do banco de dados.
/// </summary>
public class AppDbContext : DbContext
{
    private string connectionString { get; }
    
    /// <summary>
    /// Tabela de usuários. <see cref="UsuarioModel"/>
    /// </summary>
    public DbSet<UsuarioModel> usuarios { get; set; } = null!;

    /// <summary>
    /// Tabela de pessoas no sistema de gastos
    /// </summary>
    public DbSet<Pessoa> Pessoas { get; set; }
    
    /// <summary>
    /// Tabela de categorias no sistema de gastos
    /// </summary>
    public DbSet<Categoria> Categorias { get; set; }
    
    /// <summary>
    /// Tabela de transações no sistema de gastos
    /// </summary>
    public DbSet<Transacao> Transacoes { get; set; }
    
    public AppDbContext(EnvironmentService environmentService)
    {
        connectionString = environmentService.postgresConnectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql(connectionString);
}