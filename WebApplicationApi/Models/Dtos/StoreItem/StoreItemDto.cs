using WebApplicationApi.Models.DataModels;

namespace WebApplicationApi.Models.Dtos.StoreItem;

public class StoreItemDto
{
    public required ProductModel? Product { get; set; }

    public required int? AvailableQuantity { get; set; }

    public int? BookedQuantity { get; set; }

    public int? SoldQuantity { get; set; }
}
