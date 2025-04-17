using WebApplicationApi.Enums;

namespace WebApplicationApi.Models.Dtos;

public class LoginResponseDto
{
    public string Token { get; set; }

    public Role Role { get; set; }

    public int UserId { get; set; }
}
