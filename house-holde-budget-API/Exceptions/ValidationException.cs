// Exceptions/ValidationException.cs (se não existir, crie)
namespace HouseholdBudgetApi.Exceptions;

public class ValidationException : Exception
{
    public IEnumerable<string> Errors { get; }

    public ValidationException(string message) : base(message)
    {
        Errors = new List<string> { message };
    }

    public ValidationException(IEnumerable<string> errors) 
        : base(string.Join("; ", errors))
    {
        Errors = errors;
    }

    // Adicione este construtor para compatibilidade com FluentValidation
    public ValidationException(IEnumerable<FluentValidation.Results.ValidationFailure> failures)
        : base(string.Join("; ", failures.Select(f => f.ErrorMessage)))
    {
        Errors = failures.Select(f => f.ErrorMessage).ToList();
    }
}