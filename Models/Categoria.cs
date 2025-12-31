using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HouseHoldeBudgetApi.Models;

namespace HouseholdBudgetApi.Models;

/// <summary>
/// Representa uma categoria de transação
/// </summary>
[Table("Categorias")] // Nome da tabela no banco
public class Categoria
{
    /// <summary>
    /// Identificador único da categoria (auto-gerado)
    /// </summary>
    [Key] // Chave primária
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment
    [Column("Id")] // Nome da coluna no banco
    public int Id { get; set; }
    
    /// <summary>
    /// Descrição da categoria (ex: "Alimentação", "Salário")
    /// </summary>
    [Required(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Descrição deve ter entre 3 e 50 caracteres")]
    [Column("Descricao", TypeName = "varchar(50)")]
    public string Descricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Finalidade da categoria (despesa, receita ou ambas)
    /// </summary>
    [Required(ErrorMessage = "Finalidade é obrigatória")]
    [Column("Finalidade", TypeName = "varchar(10)")]
    public FinalidadeCategoria Finalidade { get; set; }
    
    /// <summary>
    /// Lista de transações associadas a esta categoria (propriedade de navegação)
    /// </summary>
    [InverseProperty("Categoria")] // Especifica a propriedade de navegação inversa
    public virtual ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
}