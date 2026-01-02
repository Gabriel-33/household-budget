using HouseHoldeBudgetApi.Models.Enums;

namespace HouseHoldeBudgetApi.Models;

public class UpdateUserRequestModel
{
    public string? username { get; set; }
    public string? password { get; set; }
    public UserRole? role { get; set; }
}