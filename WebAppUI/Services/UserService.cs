using WebApplicationApi.Authorization;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Models.Dtos.User;

namespace WebAppUI.Services;

public class UserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        var token = new TokenGenerator().GenerateSuperUserJwtToken();
        _httpClient.DefaultRequestHeaders.Add(Config.ApiKeyHeader, Config.ApiKey);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }

    public async Task<List<UserModel>> GetUsersAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<UserModel>>("api/Users");
    }

    public async Task<UserModel?> GetUserByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<UserModel>($"api/Users/{id}");
    }

    public async Task<UserModel> AddUserAsync(UserDto userDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Users", userDto);
        return await response.Content.ReadFromJsonAsync<UserModel>();
    }

    public async Task<UserModel> EditUserAsync(int id, UserDto userDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Users/{id}", userDto);
        return await response.Content.ReadFromJsonAsync<UserModel>();
    }
}
