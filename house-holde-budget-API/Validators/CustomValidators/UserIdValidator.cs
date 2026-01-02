namespace HouseHoldeBudgetApi.Validators.CustomValidators;

public record UserIdValidator(int id)
{
    public bool isValid => id > 0;
}