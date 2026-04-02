namespace HouseHoldeBudgetApi.Exceptions;

public class UsuarioNotFoundException : Exception
{
    private const string MESSAGE = "Usuário de email {1} não encontrado";

    public UsuarioNotFoundException(string parameterName, string parameterValue) : 
        base(string.Format(MESSAGE, parameterName, parameterValue)) { }
}