using Riok.Mapperly.Abstractions;
using HouseHoldeBudgetApi.Models;

namespace HouseHoldeBudgetApi.Mapper;

[Mapper]
public partial class ResetUserPasswordRequestModelMapper
{
    public partial ResetUserPasswordReadModel ResetUserPasswordRequestModelToResetUserPasswordReadModel(ResetUserPasswordRequestModel model);
}