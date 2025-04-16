using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApplicationApi.Enums;
using WebApplicationApi.Models.Dtos;

namespace WebApplicationApi.Authorization;

public class TokenGenerator
{
    private readonly string _key = Config.SecurityKey;

    public string GenerateSuperUserJwtToken()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Role, nameof(Role.Customer)),
            new Claim(ClaimTypes.Role, nameof(Role.Admin)),
            new Claim(ClaimTypes.Role, nameof(Role.Manager)),
        };

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateJwtToken(Role? role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Role, role.ToString())
        };

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
