using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HouseHoldeBudgetApi.Models;

namespace HouseholdBudgetApi.Models;

/// <summary>
/// Representa uma pessoa no sistema de gastos
/// </summary>
[Table("Pessoas")]
public class Pessoa
{
    /// <summary>
    /// Identificador único da pessoa (auto-gerado)
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int Id { get; set; }
    
    /// <summary>
    /// Nome completo da pessoa
    /// </summary>
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
    [Column("Nome", TypeName = "varchar(100)")]
    public string Nome { get; set; } = string.Empty;
    
    /// <summary>
    /// Idade da pessoa (número inteiro positivo)
    /// </summary>
    [Required(ErrorMessage = "Idade é obrigatória")]
    [Range(0, 150, ErrorMessage = "Idade deve estar entre 0 e 150 anos")]
    [Column("Idade")]
    public int Idade { get; set; }
    
    /// <summary>
    /// Data de criação do registro
    /// </summary>
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)] // Valor gerado pelo banco
    [Column("CreatedAt", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Data da última atualização do registro
    /// </summary>
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)] // Valor gerado pelo banco
    [Column("UpdatedAt", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Lista de transações associadas à pessoa
    /// </summary>
    [InverseProperty("Pessoa")]
    public virtual ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
}