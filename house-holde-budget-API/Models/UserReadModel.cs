using HouseHoldeBudgetApi.Models.Enums;

namespace HouseHoldeBudgetApi.Models;

public class UserReadModel
{
    public required int id { get; init; }
    public required string username { get; init; }
    public required string email { get; init; }
    public required UserRole role { get; init; }
}