using HouseHoldeBudgetApi.Services.Email.Models.Template;

namespace HouseHoldeBudgetApi.Services.Email.Models;

public class EmailIntent
{
    public required string toEmail { get; init; }
    public required IEmailTemplate template { get; init; }
    public string? subject { get; init; }
}