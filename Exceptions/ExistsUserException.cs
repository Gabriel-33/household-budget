using HouseHoldeBudgetApi.Utils.Extensions;

namespace HouseHoldeBudgetApi.Exceptions;

public class ExistsUserException : Exception
{
    private const string MESSAGE = "Já existe um usuário com o email: [{0}]";

    public ExistsUserException(string email)
    : base(string.Format(MESSAGE, email)) { }

}