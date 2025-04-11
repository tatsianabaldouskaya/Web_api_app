using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Models.Dtos.Product;

namespace WebAppUI.Services;

public class ProductService : BaseService
{
    public ProductService(IHttpClientFactory httpClient, AuthService authService) : base(httpClient, authService)
    {
    }

    public async Task<List<ProductModel>> GetProductsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ProductModel>>("api/Products");
    }

    public async Task<ProductModel> GetProductByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ProductModel>($"api/Products/{id}");
    }

    public async Task<ProductModel> CreateProductAsync(ProductDto productDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Products", productDto);
        return await response.Content.ReadFromJsonAsync<ProductModel>();
    }

    public async Task<ProductModel> UpdateProductAsync(int id, ProductDto productDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Products/{id}", productDto);
        return await response.Content.ReadFromJsonAsync<ProductModel>();
    }
}
