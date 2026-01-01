// Validators/CreateTransacaoRequestModelValidator.cs
using FluentValidation;
using HouseHoldeBudgetApi.Models.Requests;

namespace HouseholdBudgetApi.Validators;

/// <summary>
/// Validator para o modelo de criação de transação.
/// </summary>
public class CreateTransacaoRequestModelValidator : AbstractValidator<CreateTransacaoRequestModel>
{
    public CreateTransacaoRequestModelValidator()
    {
        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("Descrição é obrigatória")
            .MaximumLength(200).WithMessage("Descrição não pode ter mais de 200 caracteres");

        RuleFor(x => x.Valor)
            .GreaterThan(0).WithMessage("Valor deve ser maior que zero")
            .PrecisionScale(18, 2, false).WithMessage("Valor deve ter no máximo 2 casas decimais");

        RuleFor(x => x.Tipo)
            .IsInEnum().WithMessage("Tipo de transação inválido. Use 'Despesa' ou 'Receita'");

        RuleFor(x => x.CategoriaId)
            .GreaterThan(0).WithMessage("CategoriaId deve ser maior que zero");

        RuleFor(x => x.PessoaId)
            .GreaterThan(0).WithMessage("PessoaId deve ser maior que zero");
    }
}