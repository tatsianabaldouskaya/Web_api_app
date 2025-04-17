using WebApplicationApi.Models.DataModels;

namespace WebAppUI.Services;

public class StoreItemsService : BaseService
{
    public StoreItemsService(IHttpClientFactory httpClient, AuthService authService) : base(httpClient, authService)
    {
    }

    public async Task<List<StoreItemModel>> GetStoreItemsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<StoreItemModel>>("api/StoreItems");
    }
}
