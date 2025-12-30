using HouseHoldeBudgetApi.Utils.Extensions;

namespace HouseHoldeBudgetApi.Exceptions;

public class ValidationException : Exception
{
    private const string MESSAGE = "{0}";

    public ValidationException(IEnumerable<string> errors) : 
        base(string.Format(MESSAGE, errors.ToSeparatedString())) { }
    public ValidationException(string error) : 
        base(string.Format(MESSAGE, error)) { }
}