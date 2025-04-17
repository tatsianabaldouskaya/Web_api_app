namespace Taf.Dtos;

public class AddProductRequestDto
{
    public required string? Name { get; set; }

    public string? Description { get; set; }

    public required string? Author { get; set; }

    public required float? Price { get; set; }

    public string? ImagePath { get; set; }
}
