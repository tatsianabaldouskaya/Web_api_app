using WebApplicationApi.Enums;
using WebApplicationApi.Models.DataModels;

namespace WebApplicationApi.Models.Dtos.Booking;

public class BookingDto
{
    public required ProductModel? Product { get; set; }

    public string? Description { get; set; }

    public required UserModel? User { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? Time { get; set; }

    public required BookingStatus? Status { get; set; }

    public required int? Quantity { get; set; }
}
