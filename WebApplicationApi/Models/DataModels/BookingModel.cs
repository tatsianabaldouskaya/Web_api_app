using System.ComponentModel.DataAnnotations.Schema;

using WebApplicationApi.Enums;

namespace WebApplicationApi.Models.DataModels;

public class BookingModel
{
    public int? Id { get; set; }

    public int? UserId { get; set; } // foreign key

    public int? ProductId { get; set; } // foreign key

    public string? DeliveryAddress { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? Time { get; set; }

    public int? StatusId { get; set; } // foreign key

    public int? Quantity { get; set; }
}
