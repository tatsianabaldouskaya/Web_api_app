using WebApplicationApi.Authorization;

namespace WebAppUI.Services;

public class BaseService
{
    protected readonly HttpClient _httpClient;

    public BaseService(IHttpClientFactory httpClientFactory, AuthService authService)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");

        if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
        {
            var token = authService.Token;
            _httpClient.DefaultRequestHeaders.Add(Config.ApiKeyHeader, Config.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
    }
}
