namespace HouseHoldeBudgetApi.Models.Responses;

/// <summary>
/// Modelo de resposta para relatório de totais por pessoa
/// </summary>
public class RelatorioPessoasResponse
{
    /// <summary>
    /// Lista de totais por pessoa
    /// </summary>
    public List<PessoaTotal> Pessoas { get; set; } = new();
    
    /// <summary>
    /// Totais gerais consolidados
    /// </summary>
    public TotalGeral TotalGeral { get; set; } = new();
}

/// <summary>
/// Representa os totais de uma pessoa específica
/// </summary>
public class PessoaTotal
{
    /// <summary>
    /// ID da pessoa
    /// </summary>
    public int PessoaId { get; set; }
    
    /// <summary>
    /// Nome da pessoa
    /// </summary>
    public string Nome { get; set; } = string.Empty;
    
    /// <summary>
    /// Idade da pessoa
    /// </summary>
    public int Idade { get; set; }
    
    /// <summary>
    /// Total de receitas da pessoa
    /// </summary>
    public decimal TotalReceitas { get; set; }
    
    /// <summary>
    /// Total de despesas da pessoa
    /// </summary>
    public decimal TotalDespesas { get; set; }
    
    /// <summary>
    /// Saldo da pessoa (receitas - despesas)
    /// </summary>
    public decimal Saldo => TotalReceitas - TotalDespesas;
}

/// <summary>
/// Representa os totais gerais do sistema
/// </summary>
public class TotalGeral
{
    /// <summary>
    /// Total geral de receitas
    /// </summary>
    public decimal TotalReceitas { get; set; }
    
    /// <summary>
    /// Total geral de despesas
    /// </summary>
    public decimal TotalDespesas { get; set; }
    
    /// <summary>
    /// Saldo líquido geral
    /// </summary>
    public decimal SaldoLiquido => TotalReceitas - TotalDespesas;
}