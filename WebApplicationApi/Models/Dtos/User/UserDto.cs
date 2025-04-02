using WebApplicationApi.Enums;

namespace WebApplicationApi.Models.Dtos.User;

public class UserDto
{
    public required string? Name { get; set; }

    public required Role? Role { get; set; }

    public required string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }
}
