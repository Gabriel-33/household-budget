using HouseHoldeBudgetApi.Utils.Extensions;

namespace HouseHoldeBudgetApi.Exceptions;

public class ExistsUserException : Exception
{
    private const string MESSAGE = "Já existe um usuário com a matrícula: [{0}] ou o email: [{1}]";

    public ExistsUserException(string matricula, string email)
    : base(string.Format(MESSAGE, matricula, email)) { }

}