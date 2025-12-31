using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HouseHoldeBudgetApi.Models;

namespace HouseholdBudgetApi.Models;

/// <summary>
/// Representa uma transação financeira
/// </summary>
[Table("Transacoes")]
public class Transacao
{
    /// <summary>
    /// Identificador único da transação (auto-gerado)
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int Id { get; set; }
    
    /// <summary>
    /// Descrição da transação
    /// </summary>
    [Required(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Descrição deve ter entre 3 e 200 caracteres")]
    [Column("Descricao", TypeName = "varchar(200)")]
    public string Descricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Valor da transação (número decimal positivo)
    /// </summary>
    [Required(ErrorMessage = "Valor é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
    [Column("Valor", TypeName = "decimal(18,2)")]
    public decimal Valor { get; set; }
    
    /// <summary>
    /// Tipo da transação (despesa ou receita)
    /// </summary>
    [Required(ErrorMessage = "Tipo é obrigatório")]
    [Column("Tipo", TypeName = "varchar(10)")]
    public TipoTransacao Tipo { get; set; }
    
    /// <summary>
    /// Data da transação
    /// </summary>
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Column("Data", TypeName = "timestamp")]
    public DateTime Data { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// ID da categoria da transação (chave estrangeira)
    /// </summary>
    [Required(ErrorMessage = "Categoria é obrigatória")]
    [ForeignKey("Categoria")] // Define a relação de chave estrangeira
    [Column("CategoriaId")]
    public int CategoriaId { get; set; }
    
    /// <summary>
    /// ID da pessoa associada à transação (chave estrangeira)
    /// </summary>
    [Required(ErrorMessage = "Pessoa é obrigatória")]
    [ForeignKey("Pessoa")] // Define a relação de chave estrangeira
    [Column("PessoaId")]
    public int PessoaId { get; set; }
    
    /// <summary>
    /// Categoria da transação (propriedade de navegação)
    /// </summary>
    [InverseProperty("Transacoes")]
    public virtual Categoria Categoria { get; set; } = null!;
    
    /// <summary>
    /// Pessoa associada à transação (propriedade de navegação)
    /// </summary>
    [InverseProperty("Transacoes")]
    public virtual Pessoa Pessoa { get; set; } = null!;
}