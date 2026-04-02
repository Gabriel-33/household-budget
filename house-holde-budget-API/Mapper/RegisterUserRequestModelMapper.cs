using Riok.Mapperly.Abstractions;
using HouseHoldeBudgetApi.Models;

namespace HouseHoldeBudgetApi.Mapper;

[Mapper]
public partial class RegisterUserRequestModelMapper
{
    [MapProperty(nameof(RegisterUserRequestModel.username), nameof(UsuarioModel.nomeUsuario))]
    [MapProperty(nameof(RegisterUserRequestModel.email), nameof(UsuarioModel.emailUsuario))]
    [MapProperty(nameof(RegisterUserRequestModel.password), nameof(UsuarioModel.senhaUsuario))]
    [MapProperty(nameof(RegisterUserRequestModel.role), nameof(UsuarioModel.tipoUsuario))]
    public partial UsuarioModel RegisterUserRequestModelToUsuarioModel(RegisterUserRequestModel model);
}