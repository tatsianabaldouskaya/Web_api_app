using WebApplicationApi.Authorization;

namespace WebAppUI.Services;

public class BaseService
{
    protected readonly HttpClient _httpClient;

    public BaseService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");

        if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
        {
            var token = new TokenGenerator().GenerateSuperUserJwtToken();
            _httpClient.DefaultRequestHeaders.Add(Config.ApiKeyHeader, Config.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
    }
}
