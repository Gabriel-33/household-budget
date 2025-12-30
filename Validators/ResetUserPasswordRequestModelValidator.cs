using FluentValidation;
using HouseHoldeBudgetApi.Models;

namespace HouseHoldeBudgetApi.Validators;

public class ResetUserPasswordRequestModelValidator : AbstractValidator<ResetUserPasswordRequestModel>
{
    public ResetUserPasswordRequestModelValidator()
    {
        RuleFor(m => m.newPassword)
            .NotNull()
            .NotEmpty()
            //.MinimumLength(8)
            .MaximumLength(60);
        RuleFor(m => m.resetCode)
            .NotNull()
            .NotEmpty();
    }
}