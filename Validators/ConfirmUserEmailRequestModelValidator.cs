using FluentValidation;
using HouseHoldeBudgetApi.Models;

namespace HouseHoldeBudgetApi.Validators;

public class ConfirmUserEmailRequestModelValidator : AbstractValidator<ConfirmUserEmailRequestModel>
{
    public ConfirmUserEmailRequestModelValidator()
    {
        RuleFor(m => m.confirmationCode)
            .NotNull()
            .NotEmpty();
    }
}