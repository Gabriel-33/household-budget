namespace HouseHoldeBudgetApi.Exceptions;

/// <summary>
/// Exceção lançada quando uma validação de transação falha
/// </summary>
public class TransacaoValidationException : Exception
{
    /// <summary>
    /// Lista de erros de validação
    /// </summary>
    public List<string> Errors { get; }

    /// <summary>
    /// Construtor da exceção
    /// </summary>
    /// <param name="message">Mensagem de erro</param>
    public TransacaoValidationException(string message) : base(message)
    {
        Errors = new List<string> { message };
    }

    /// <summary>
    /// Construtor da exceção com múltiplos erros
    /// </summary>
    /// <param name="errors">Lista de erros</param>
    public TransacaoValidationException(List<string> errors)
        : base(string.Join("; ", errors))
    {
        Errors = errors;
    }
}