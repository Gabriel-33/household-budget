namespace HouseHoldeBudgetApi.Exceptions;

/// <summary>
/// Exceção lançada quando uma categoria não é encontrada
/// </summary>
public class CategoriaNotFoundException : Exception
{
    /// <summary>
    /// ID da categoria não encontrada
    /// </summary>
    public int CategoriaId { get; }

    /// <summary>
    /// Construtor da exceção
    /// </summary>
    /// <param name="categoriaId">ID da categoria não encontrada</param>
    public CategoriaNotFoundException(int categoriaId)
        : base($"Categoria com ID {categoriaId} não encontrada")
    {
        CategoriaId = categoriaId;
    }

    /// <summary>
    /// Construtor da exceção com mensagem personalizada
    /// </summary>
    /// <param name="categoriaId">ID da categoria não encontrada</param>
    /// <param name="message">Mensagem personalizada</param>
    public CategoriaNotFoundException(int categoriaId, string message)
        : base(message)
    {
        CategoriaId = categoriaId;
    }
}