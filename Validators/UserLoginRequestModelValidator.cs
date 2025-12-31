using FluentValidation;
using HouseHoldeBudgetApi.Models;
using HouseHoldeBudgetApi.Validators.CustomValidators;

namespace HouseHoldeBudgetApi.Validators;

public class UserLoginRequestModelValidator : AbstractValidator<UserLoginRequestModel>
{
    public UserLoginRequestModelValidator()
    {
        RuleFor(m => m.email)
            .NotEmpty();
        RuleFor(m => m.password)
            .NotNull()
            .NotEmpty()
            .MinimumLength(8);
    }
}