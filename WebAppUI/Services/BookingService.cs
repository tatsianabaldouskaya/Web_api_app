using WebApplicationApi.Authorization;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Models.Dtos.Booking;

namespace WebAppUI.Services;

public class BookingService
{
    private readonly HttpClient _httpClient;

    public BookingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        var token = new TokenGenerator().GenerateSuperUserJwtToken();
        _httpClient.DefaultRequestHeaders.Add(Config.ApiKeyHeader, Config.ApiKey);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }
    public async Task<List<BookingModel>> GetBookingsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<BookingModel>>("api/Bookings");
    }

    public async Task<BookingModel?> GetBookingByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<BookingModel>($"api/Bookings/{id}");
    }

    public async Task<BookingModel> CreateBookingAsync(BookingDto bookingDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Bookings", bookingDto);
        return await response.Content.ReadFromJsonAsync<BookingModel>();
    }

    public async Task<BookingModel> UpdateBookingAsync(int id, BookingDto bookingDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Bookings/{id}", bookingDto);
        return await response.Content.ReadFromJsonAsync<BookingModel>();
    }
}
