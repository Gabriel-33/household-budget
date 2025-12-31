// Validators/CreatePessoaRequestModelValidator.cs
using FluentValidation;
using HouseHoldeBudgetApi.Models.Requests;

namespace HouseholdBudgetApi.Validators;

/// <summary>
/// Validador para o modelo de criação de pessoa no sistema de gastos.
/// </summary>
public class CreatePessoaRequestModelValidator : AbstractValidator<CreatePessoaRequestModel>
{
    public CreatePessoaRequestModelValidator()
    {
        // Regra para Nome
        RuleFor(x => x.Nome)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(100).WithMessage("Nome não pode ter mais de 100 caracteres")
            .Must(BeAValidName).WithMessage("Nome contém caracteres inválidos")
            .WithName("Nome");

        // Regra para Idade
        RuleFor(x => x.Idade)
            .Cascade(CascadeMode.Stop)
            .GreaterThanOrEqualTo(0).WithMessage("Idade não pode ser negativa")
            .LessThanOrEqualTo(150).WithMessage("Idade não pode ser maior que 150")
            .Must(BeAValidAge).WithMessage("Idade deve ser um número inteiro válido")
            .WithName("Idade");
    }

    /// <summary>
    /// Valida se o nome contém apenas caracteres permitidos.
    /// </summary>
    private bool BeAValidName(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return false;

        // Permite letras, acentos, espaços e alguns caracteres especiais
        return System.Text.RegularExpressions.Regex.IsMatch(nome, 
            @"^[a-zA-ZÀ-ÿ\s\-'.]+$");
    }

    /// <summary>
    /// Valida se a idade é um número inteiro válido.
    /// </summary>
    private bool BeAValidAge(int idade)
    {
        return idade >= 0 && idade <= 150;
    }
}