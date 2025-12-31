using FluentValidation;
using HouseHoldeBudgetApi.Models;
using HouseHoldeBudgetApi.Validators.CustomValidators;

namespace HouseHoldeBudgetApi.Validators;

public class RegisterUserRequestModelValidator : AbstractValidator<RegisterUserRequestModel>
{
    public RegisterUserRequestModelValidator()
    {
        RuleFor(m => m.username)
            .NotNull()
            .NotEmpty()
            .MaximumLength(45);
        RuleFor(m => m.email)
            .NotNull();
        RuleFor(m => m.password)
            .NotNull()
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(20);
    }
}