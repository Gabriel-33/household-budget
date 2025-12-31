namespace HouseHoldeBudgetApi.Exceptions;

/// <summary>
/// Exceção lançada quando uma pessoa não é encontrada
/// </summary>
public class PessoaNotFoundException : Exception
{
    /// <summary>
    /// ID da pessoa não encontrada
    /// </summary>
    public int PessoaId { get; }

    /// <summary>
    /// Construtor da exceção
    /// </summary>
    /// <param name="pessoaId">ID da pessoa não encontrada</param>
    public PessoaNotFoundException(int pessoaId)
        : base($"Pessoa com ID {pessoaId} não encontrada")
    {
        PessoaId = pessoaId;
    }

    /// <summary>
    /// Construtor da exceção com mensagem personalizada
    /// </summary>
    /// <param name="pessoaId">ID da pessoa não encontrada</param>
    /// <param name="message">Mensagem personalizada</param>
    public PessoaNotFoundException(int pessoaId, string message)
        : base(message)
    {
        PessoaId = pessoaId;
    }
}