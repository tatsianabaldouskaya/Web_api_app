namespace WebApplicationApi.Models.DataModels;

public class UserModel
{
    public int? Id { get; set; }

    public string? Name { get; set; }

    public int? RoleId { get; set; } // foreign key

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }
}
