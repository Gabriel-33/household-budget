using HouseholdBudgetApi.Models;
using HouseHoldeBudgetApi.Models;
using HouseHoldeBudgetApi.Models.Requests;
using HouseHoldeBudgetApi.Models.Responses;

namespace HouseHoldeBudgetApi.Mapper;

/// <summary>
/// Mapeador para converter entre modelos de transação
/// </summary>
public class TransacaoModelMapper
{
    /// <summary>
    /// Converte uma entidade Transacao para TransacaoReadModel
    /// </summary>
    /// <param name="transacao">Entidade Transacao</param>
    /// <returns>TransacaoReadModel</returns>
    public TransacaoReadModel TransacaoToTransacaoReadModel(Transacao transacao)
    {
        if (transacao == null)
            throw new ArgumentNullException(nameof(transacao));

        return new TransacaoReadModel
        {
            Id = transacao.Id,
            Descricao = transacao.Descricao,
            Valor = transacao.Valor,
            Tipo = transacao.Tipo,
            Data = transacao.Data,
            CategoriaDescricao = transacao.Categoria?.Descricao ?? "Não informada",
            PessoaNome = transacao.Pessoa?.Nome ?? "Não informada"
        };
    }

    /// <summary>
    /// Converte uma lista de entidades Transacao para lista de TransacaoReadModel
    /// </summary>
    /// <param name="transacoes">Lista de entidades Transacao</param>
    /// <returns>Lista de TransacaoReadModel</returns>
    public List<TransacaoReadModel> TransacoesToTransacaoReadModels(IEnumerable<Transacao> transacoes)
    {
        return transacoes.Select(TransacaoToTransacaoReadModel).ToList();
    }

    /// <summary>
    /// Converte CreateTransacaoRequestModel para entidade Transacao
    /// </summary>
    /// <param name="request">Modelo de requisição</param>
    /// <returns>Entidade Transacao</returns>
    public Transacao CreateTransacaoRequestModelToTransacao(CreateTransacaoRequestModel request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        return new Transacao
        {
            Descricao = request.Descricao.Trim(),
            Valor = request.Valor,
            Tipo = request.Tipo,
            CategoriaId = request.CategoriaId,
            PessoaId = request.PessoaId,
            Data = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Converte CreateTransacaoRequestModel para entidade Transacao com validação
    /// </summary>
    /// <param name="request">Modelo de requisição</param>
    /// <param name="pessoa">Pessoa associada</param>
    /// <param name="categoria">Categoria associada</param>
    /// <returns>Entidade Transacao</returns>
    public Transacao CreateTransacaoRequestModelToTransacaoWithEntities(
        CreateTransacaoRequestModel request,
        Pessoa pessoa,
        Categoria categoria)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        if (pessoa == null)
            throw new ArgumentNullException(nameof(pessoa));
        if (categoria == null)
            throw new ArgumentNullException(nameof(categoria));

        return new Transacao
        {
             Descricao = request.Descricao.Trim(),
             Valor = request.Valor,
             Tipo = request.Tipo,
             PessoaId = request.PessoaId,  // Apenas o ID
             CategoriaId = request.CategoriaId, // Apenas o ID
             Data = DateTime.UtcNow
        };
    }
}