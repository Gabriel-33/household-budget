// Summaries/GastosSummaries.cs
using Microsoft.OpenApi.Models;

namespace HouseholdBudgetApi.Summaries;

/// <summary>
/// Especificações OpenAPI para os endpoints do sistema de gastos.
/// </summary>
public static class GastosSummaries
{
    /// <summary>
    /// Especificação para GET /pessoas - Listar pessoas
    /// </summary>
    public static OpenApiOperation AdminGetPessoasSpecification(OpenApiOperation operation)
    {
        operation.Summary = "Listar todas as pessoas";
        operation.Description = "Retorna todas as pessoas cadastradas no sistema de gastos com suporte a paginação. Requer permissão de administrador.";
        return operation;
    }

    /// <summary>
    /// Especificação para POST /pessoas - Criar pessoa
    /// </summary>
    public static OpenApiOperation AdminCreatePessoaSpecification(OpenApiOperation operation)
    {
        operation.Summary = "Criar nova pessoa";
        operation.Description = "Cadastra uma nova pessoa no sistema de gastos. Requer permissão de administrador.";
        return operation;
    }

    /// <summary>
    /// Especificação para DELETE /pessoas/{id} - Deletar pessoa
    /// </summary>
    public static OpenApiOperation AdminDeletePessoaSpecification(OpenApiOperation operation)
    {
        operation.Summary = "Deletar pessoa";
        operation.Description = "Deleta uma pessoa e todas as suas transações associadas (cascade delete). Requer permissão de administrador.";
        return operation;
    }

    /// <summary>
    /// Especificação para GET /categorias - Listar categorias
    /// </summary>
    public static OpenApiOperation AdminGetCategoriasSpecification(OpenApiOperation operation)
    {
        operation.Summary = "Listar categorias";
        operation.Description = "Retorna todas as categorias disponíveis para transações. Requer permissão de administrador.";
        return operation;
    }

    /// <summary>
    /// Especificação para POST /categorias - Criar categoria
    /// </summary>
    public static OpenApiOperation AdminCreateCategoriaSpecification(OpenApiOperation operation)
    {
        operation.Summary = "Criar categoria";
        operation.Description = "Cadastra uma nova categoria para transações. Requer permissão de administrador.";
        return operation;
    }

    /// <summary>
    /// Especificação para GET /transacoes - Listar transações
    /// </summary>
    public static OpenApiOperation UserGetTransacoesSpecification(OpenApiOperation operation)
    {
        operation.Summary = "Listar transações";
        operation.Description = "Retorna todas as transações cadastradas com suporte a paginação e filtros. Requer autenticação de usuário.";
        return operation;
    }

    /// <summary>
    /// Especificação para POST /transacoes - Criar transação
    /// </summary>
    public static OpenApiOperation UserCreateTransacaoSpecification(OpenApiOperation operation)
    {
        operation.Summary = "Criar transação";
        operation.Description = "Cadastra uma nova transação com validações automáticas (idade, compatibilidade categoria/tipo). Requer autenticação de usuário.";
        return operation;
    }

    /// <summary>
    /// Especificação para GET /relatorios/pessoas - Relatório por pessoa
    /// </summary>
    public static OpenApiOperation AdminGetRelatorioPessoasSpecification(OpenApiOperation operation)
    {
        operation.Summary = "Relatório de totais por pessoa";
        operation.Description = "Retorna o total de receitas, despesas e saldo de cada pessoa cadastrada. Requer permissão de administrador.";
        return operation;
    }

    /// <summary>
    /// Especificação para GET /relatorios/categorias - Relatório por categoria
    /// </summary>
    
    public static OpenApiOperation AdminGetRelatorioCategoriasSpecification(OpenApiOperation operation)
    {
        operation.Summary = "Relatório de totais por categoria";
        operation.Description = "Retorna o total de receitas, despesas e saldo de cada categoria cadastrada";
        return operation;
    }
    
    /// <summary>
    /// Especificação para DELETE /deleteTransaction/{id} - Deletar uma transação específica
    /// </summary>
    public static OpenApiOperation AdminDeleteTransactionSpecification(OpenApiOperation operation)
    {
        operation.Summary = "Deletar transação";
        operation.Description = "Retorna o id deletado.";
        return operation;
    }
    
    /// <summary>
    /// Especificação para GET categorias/finalidade/{finalidade} - Busca uma lista de categorias pelo filtro finalidade
    /// </summary>
    public static OpenApiOperation AdminGetCategoriasListByFinalidade(OpenApiOperation operation)
    {
        operation.Summary = "Listar categorias por finalidade";
        operation.Description = "Retorna categorias filtradas por finalidade (Despesa, Receita ou Ambas)";
        return operation;
    }
    
    /// <summary>
    /// Especificação para DELETE categorias/{id} - Delete uma categoria especifíca, e as transações associada à ela.
    /// </summary>
    public static OpenApiOperation AdminDeleteCategoriasById(OpenApiOperation operation)
    {
        operation.Summary = "Delete uma categoria pelo id, e suas transações associadas.";
        operation.Description = "Retorna o id deletado";
        return operation;
    }
}