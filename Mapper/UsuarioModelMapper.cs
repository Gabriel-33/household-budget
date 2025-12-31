using Riok.Mapperly.Abstractions;
using HouseHoldeBudgetApi.Models;

namespace HouseHoldeBudgetApi.Mapper;

[Mapper]
public partial class UsuarioModelMapper
{
    [MapProperty(nameof(UsuarioModel.idUsuario), nameof(UserReadModel.id))]
    [MapProperty(nameof(UsuarioModel.nomeUsuario), nameof(UserReadModel.username))]
    [MapProperty(nameof(UsuarioModel.emailUsuario), nameof(UserReadModel.email))]
    [MapProperty(nameof(UsuarioModel.tipoUsuario), nameof(UserReadModel.role))]
    public partial UserReadModel UsuarioModelToUserReadModel(UsuarioModel model);
}