using HouseHoldeBudgetApi.Models.Enums;

namespace HouseHoldeBudgetApi.Models;

/// <summary>
/// DTO para receber valores da api, e posteriomente mapear na entidade User
/// </summary>
public class RegisterUserRequestModel
{
    public string username { get; set; } = null!;
    public string email { get; set; } = null!;
    public string password { get; set; } = null!;
    public UserRole role { get; set; }
}