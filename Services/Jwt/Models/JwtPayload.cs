using System.Security.Claims;
using HouseHoldeBudgetApi.Models.Enums;

namespace HouseHoldeBudgetApi.Services.Jwt.Models;

public record JwtPayload(string userId, UserRole role)
{
    public ClaimsIdentity CreateClaimsIdentity()
    {
        ClaimsIdentity claims = new();
        claims.AddClaims(new Claim[]
        {
            new(ClaimTypes.Name, userId),
            new(ClaimTypes.Role, role.ToString())
        });
        
        return claims;
    }
}