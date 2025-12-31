using FluentValidation;
using HouseHoldeBudgetApi.Models;
using HouseHoldeBudgetApi.Validators.CustomValidators;

namespace HouseHoldeBudgetApi.Validators;

public class UpdateUserRequestModelValidator : AbstractValidator<UpdateUserRequestModel>
{
    public UpdateUserRequestModelValidator()
    {
        RuleFor(m => m.username)
            .NotNull()
            .NotEmpty()
            .MaximumLength(45)
            .When(f => f.username is not null);
        RuleFor(m => m.password)
            .NotNull()
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(20)
            .When(f => f.password is not null);
        RuleFor(m => m.role)
            .IsInEnum()
            .When(f => f.role is not null);
    }
}