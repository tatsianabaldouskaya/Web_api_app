using WebApplicationApi.Authorization;
using WebApplicationApi.Enums;
using WebApplicationApi.Models.Dtos;

namespace WebAppUI.Services;

public class AuthService
{
    public string? Token { get; set; }
    public Role? CurrentRole { get; set; }
    public int? UserId { get; set; }

    public void Login(LoginDto loginDto)
    {
        CurrentRole = (loginDto.Username, loginDto.Password) switch
        {
            ("admin", "admin") => Role.Admin,
            ("customer", "customer") => Role.Customer,
            ("manager", "manager") => Role.Manager,
            _ => null
        };

        if (CurrentRole != null)
        {
            Token = new TokenGenerator().GenerateJwtToken(CurrentRole.ToString());
        }
    }

    public void Logout()
    {
        Token = null;
        CurrentRole = null;
        UserId = null;
    }

    public bool IsAuthenticated()
    {
        return CurrentRole != null;
    }
}
