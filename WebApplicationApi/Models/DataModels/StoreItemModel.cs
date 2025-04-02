namespace WebApplicationApi.Models.DataModels;

public class StoreItemModel
{
    public int? Id { get; set; }

    public int? ProductId { get; set; } // foreign key

    public int? AvailableQuantity { get; set; }

    public int? BookedQuantity { get; set; }

    public int? SoldQuantity { get; set; }
}
