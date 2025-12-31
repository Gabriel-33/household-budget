// Validators/CreateCategoriaRequestModelValidator.cs
using FluentValidation;
using HouseHoldeBudgetApi.Models;
using HouseHoldeBudgetApi.Models.Requests;

namespace HouseholdBudgetApi.Validators;

/// <summary>
/// Validador para o modelo de criação de categoria no sistema de gastos.
/// </summary>
public class CreateCategoriaRequestModelValidator : AbstractValidator<CreateCategoriaRequestModel>
{
    private readonly List<string> _categoriasProibidas = new()
    {
        "teste", "test", "abc", "xyz", "123", "categoria"
    };

    public CreateCategoriaRequestModelValidator()
    {
        // Regra para Descrição
        RuleFor(x => x.Descricao)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Descrição é obrigatória")
            .MaximumLength(50).WithMessage("Descrição não pode ter mais de 50 caracteres")
            .MinimumLength(3).WithMessage("Descrição deve ter pelo menos 3 caracteres")
            .Must(BeAValidDescription).WithMessage("Descrição contém caracteres inválidos")
            .Must(NotBeProhibitedCategory).WithMessage("Descrição não pode ser uma palavra genérica")
            .WithName("Descrição");

        // Regra para Finalidade
        RuleFor(x => x.Finalidade)
            .Cascade(CascadeMode.Stop)
            .IsInEnum().WithMessage("Finalidade inválida. Use 'Despesa', 'Receita' ou 'Ambas'")
            .Must(BeAValidFinality).WithMessage("Finalidade deve ser um valor válido do enum")
            .WithName("Finalidade");
    }

    /// <summary>
    /// Valida se a descrição contém apenas caracteres permitidos.
    /// </summary>
    private bool BeAValidDescription(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            return false;

        // Permite letras, números, acentos, espaços e alguns caracteres especiais
        return System.Text.RegularExpressions.Regex.IsMatch(descricao, 
            @"^[a-zA-ZÀ-ÿ0-9\s\-_.,;:!?()]+$");
    }

    /// <summary>
    /// Valida se a categoria não está na lista de categorias proibidas.
    /// </summary>
    private bool NotBeProhibitedCategory(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            return false;

        var descricaoLower = descricao.Trim().ToLower();
        return !_categoriasProibidas.Any(proibida => 
            descricaoLower.Contains(proibida) || 
            descricaoLower == proibida);
    }

    /// <summary>
    /// Valida se a finalidade é um valor válido do enum.
    /// </summary>
    private bool BeAValidFinality(FinalidadeCategoria finalidade)
    {
        return Enum.IsDefined(typeof(FinalidadeCategoria), finalidade);
    }
}