using WebApplicationApi.Enums;
using WebApplicationApi.Models.Dtos;

namespace WebAppUI.Services;

public class AuthService : BaseService
{
    public string? Token { get; set; }
    public Role? CurrentRole { get; set; }
    public int? UserId { get; set; }

    public AuthService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public async Task Login(LoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            Token = result.Token;
            CurrentRole = result.Role;
            UserId = result.UserId;
        }
        else
        {
            CurrentRole = null;
            Token = null;
            UserId = null;
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
