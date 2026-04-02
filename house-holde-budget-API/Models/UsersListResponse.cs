namespace HouseHoldeBudgetApi.Models;
/// <summary>
/// DTO de resposta, para, trazer uma Lista de UserReadModel
/// </summary>
public class UsersListResponse
{
    public int maxPage { get; init; }
    public int usersCount { get; init; }
    public int pageCount { get; init; }
    public required IReadOnlyList<UserReadModel> users { get; init; }
}