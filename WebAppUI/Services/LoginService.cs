using WebApplicationApi.Models.Dtos;

namespace WebAppUI.Services;

public class LoginService : BaseService
{
    public LoginService(IHttpClientFactory httpClient) : base(httpClient)
    {
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Login", loginDto);
        var token = await response.Content.ReadAsStringAsync();
        return token;
    }
}
