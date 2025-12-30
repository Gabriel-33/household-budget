using HouseHoldeBudgetApi.Models.Enums;

namespace HouseHoldeBudgetApi.Models;

public class CodigoUsuarioReadModel
{
    public required string code { get; init; }
    public required UserCodeKind codeKind { get; init; }
}