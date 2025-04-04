using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebApplicationApi.Authorization;
using WebApplicationApi.Enums;
using WebApplicationApi.Models.Dtos;

namespace WebApplicationApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login(LoginDto login)
    {
        var role = GetRoleByCredentials(login.Username, login.Password);
        if (role == null)
        {
            return Unauthorized();
        }
        var token = new TokenGenerator().GenerateJwtToken(role);
        return Ok(new { token });

    }

    private string GetRoleByCredentials(string username, string password)
    {
        if (username == "admin" && password == "admin")
            return nameof(Role.Admin);
        if (username == "customer" && password == "customer")
            return nameof(Role.Customer);
        if (username == "manager" && password == "manager")
            return nameof(Role.Manager);
        else return null;
    }
}
