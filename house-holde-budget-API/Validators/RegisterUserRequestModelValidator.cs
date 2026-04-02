using FluentValidation;
using HouseHoldeBudgetApi.Models;
using HouseHoldeBudgetApi.Validators.CustomValidators;

namespace HouseHoldeBudgetApi.Validators;

/// <summary>
/// Validator para o modelo de criação de usuário.
/// </summary>
public class RegisterUserRequestModelValidator : AbstractValidator<RegisterUserRequestModel>
{
    public RegisterUserRequestModelValidator()
    {
        RuleFor(m => m.username)
            .NotEmpty().WithMessage("Username é obrigatório")
            .MaximumLength(45).WithMessage("Username não pode ter mais de 45 caracteres");
        RuleFor(m => m.email)
            .NotNull().WithMessage("Username é obrigatório");
        RuleFor(m => m.password)
            .NotEmpty().WithMessage("Password é obrigatório")
            .MinimumLength(8).WithMessage("Password não pode ter menos de 8 caracteres")
            .MaximumLength(20).WithMessage("Password não pode ter mais de 20 caracteres");;
    }
}