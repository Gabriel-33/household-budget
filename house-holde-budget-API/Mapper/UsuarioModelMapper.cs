using HouseHoldeBudgetApi.Models;

namespace HouseHoldeBudgetApi.Mapper;
public class UsuarioModelMapper
{
    public UserReadModel UsuarioModelToUserReadModel(UsuarioModel model)
    {
        if (model == null) return null;
        
        return new UserReadModel
        {
            id = model.idUsuario,
            username = model.nomeUsuario,
            email = model.emailUsuario,
            role = model.tipoUsuario
        };
    }
}