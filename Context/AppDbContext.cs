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
    /// Tabela de cursos. <see cref="CursoModel"/>
    /// </summary>
    public DbSet<CursoModel> cursos { get; set; } = null!;
    
    /// <summary>
    /// Tabela de códigos de confirmação de email e recuperação de senha. <see cref="CodigoUsuarioModel"/>
    /// </summary>
    public DbSet<CodigoUsuarioModel> codigoUsuario { get; set; } = null!;
    
    /// <summary>
    /// Tabela de disciplinas. <see cref="DisciplinaModel"/>
    /// </summary>
    public DbSet<DisciplinaModel> disciplinas { get; set; } = null!;
    
    /// <summary>
    /// Tabela de discussões. <see cref="TopicoDiscussaoModel"/>
    /// </summary>
    public DbSet<TopicoDiscussaoModel> discussao { get; set; } = null!;

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