namespace HouseHoldeBudgetApi.Models.Responses;

/// <summary>
/// Modelo de resposta para relatório de totais por categoria
/// </summary>
public class RelatorioCategoriasResponse
{
    /// <summary>
    /// Lista de totais por categoria
    /// </summary>
    public List<CategoriaTotal> Categorias { get; set; } = new();
    
    /// <summary>
    /// Totais gerais consolidados
    /// </summary>
    public TotalGeral TotalGeral { get; set; } = new();
}

/// <summary>
/// Representa os totais de uma categoria específica
/// </summary>
public class CategoriaTotal
{
    /// <summary>
    /// ID da categoria
    /// </summary>
    public int CategoriaId { get; set; }
    
    /// <summary>
    /// Descrição da categoria
    /// </summary>
    public string Descricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Finalidade da categoria
    /// </summary>
    public FinalidadeCategoria Finalidade { get; set; }
    
    /// <summary>
    /// Total de receitas na categoria
    /// </summary>
    public decimal TotalReceitas { get; set; }
    
    /// <summary>
    /// Total de despesas na categoria
    /// </summary>
    public decimal TotalDespesas { get; set; }
    
    /// <summary>
    /// Saldo da categoria (receitas - despesas)
    /// </summary>
    public decimal Saldo => TotalReceitas - TotalDespesas;
}