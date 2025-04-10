using Newtonsoft.Json;
using System.Text;

using WebApplicationApi.Authorization;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Models.Dtos.User;

namespace WebAppUI.Services;

public class UserService : BaseService
{
    public UserService(IHttpClientFactory httpClient) : base(httpClient)
    {
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
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserModel>();
    }
}
