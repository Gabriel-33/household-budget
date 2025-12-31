using HouseHoldeBudgetApi.Models;
using HouseHoldeBudgetApi.Models.Requests;
using HouseHoldeBudgetApi.Models.Responses;

namespace HouseHoldeBudgetApi.Mapper;

public class PessoaModelMapper
{
       /// <summary>
    /// Converte uma entidade Pessoa para PessoaReadModel
    /// </summary>
    /// <param name="pessoa">Entidade Pessoa</param>
    /// <returns>PessoaReadModel</returns>
    public PessoaReadModel PessoaToPessoaReadModel(Pessoa pessoa)
    {
        if (pessoa == null)
            throw new ArgumentNullException(nameof(pessoa));

        return new PessoaReadModel
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Idade = pessoa.Idade,
            TotalTransacoes = pessoa.Transacoes?.Count ?? 0
        };
    }

    /// <summary>
    /// Converte uma lista de entidades Pessoa para lista de PessoaReadModel
    /// </summary>
    /// <param name="pessoas">Lista de entidades Pessoa</param>
    /// <returns>Lista de PessoaReadModel</returns>
    public List<PessoaReadModel> PessoasToPessoaReadModels(IEnumerable<Pessoa> pessoas)
    {
        return pessoas.Select(PessoaToPessoaReadModel).ToList();
    }

    /// <summary>
    /// Converte CreatePessoaRequestModel para entidade Pessoa
    /// </summary>
    /// <param name="request">Modelo de requisição</param>
    /// <returns>Entidade Pessoa</returns>
    public Pessoa CreatePessoaRequestModelToPessoa(CreatePessoaRequestModel request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        return new Pessoa
        {
            Nome = request.Nome.Trim(),
            Idade = request.Idade,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Converte PessoaReadModel para PessoaTotal (usado em relatórios)
    /// </summary>
    /// <param name="pessoa">Entidade Pessoa</param>
    /// <param name="totalReceitas">Total de receitas</param>
    /// <param name="totalDespesas">Total de despesas</param>
    /// <returns>PessoaTotal</returns>
    public PessoaTotal PessoaToPessoaTotal(Pessoa pessoa, decimal totalReceitas, decimal totalDespesas)
    {
        if (pessoa == null)
            throw new ArgumentNullException(nameof(pessoa));

        return new PessoaTotal
        {
            PessoaId = pessoa.Id,
            Nome = pessoa.Nome,
            Idade = pessoa.Idade,
            TotalReceitas = totalReceitas,
            TotalDespesas = totalDespesas
        };
    }
}