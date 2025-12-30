using HouseHoldeBudgetApi.Services.Email.Models;

namespace HouseHoldeBudgetApi.Services.Email;

public interface IEmailService
{
    public Task SendEmail(EmailIntent intent, bool useEdgeFunction = true);
    
}