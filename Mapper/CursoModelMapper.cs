using Riok.Mapperly.Abstractions;
using HouseHoldeBudgetApi.Models;

namespace HouseHoldeBudgetApi.Mapper;

[Mapper]
public partial class CursoModelMapper
{
    [MapperIgnoreSource(nameof(CursoModel.idCurso))]
    [MapProperty(nameof(CursoModel.nomeCurso), nameof(CursoReadModel.nome))]
    public partial CursoReadModel CursoModelToCursoReadModel(CursoModel model);
}