using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using WebApplicationApi.Enums;
using WebApplicationApi.Models.Dtos;

namespace WebAppUI.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(ProtectedSessionStorage sessionStorage, ClaimsPrincipal anonymous)
    {
        _sessionStorage = sessionStorage;
        _anonymous = anonymous;
    }

    private string GetRoleByCredentials(LoginDto loginDto)
    {
        if ( loginDto.Username== "admin" && loginDto.Password == "admin")
            return nameof(Role.Admin);
        if (loginDto.Username == "customer" && loginDto.Password == "customer")
            return nameof(Role.Customer);
        if (loginDto.Username == "manager" && loginDto.Password == "manager")
            return nameof(Role.Manager);
        else return null;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        throw new NotImplementedException();
    }
}
