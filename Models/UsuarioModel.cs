using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HouseHoldeBudgetApi.Models.Enums;

namespace HouseHoldeBudgetApi.Models;

[Table("usuario")]
public class UsuarioModel
{
    [Key]
    [Column("id_usuario")]
    public int idUsuario { get; set; }

    [Column("nome_usuario")]
    [Required]
    public string nomeUsuario { get; set; }
    
    [Column("email_usuario")]
    [MaxLength(60)]
    [Required]
    public string emailUsuario { get; set; }

    [Column("senha_usuario")]
    [MaxLength(60)]
    [Required]
    public string senhaUsuario { get; set; }
    
    [Column("tipo_usuario")]
    [Required]
    public UserRole tipoUsuario { get; set; }
}