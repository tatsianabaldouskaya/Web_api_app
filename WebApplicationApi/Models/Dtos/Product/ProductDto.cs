﻿namespace WebApplicationApi.Models.Dtos.Product;

public class ProductDto
{
    public required string? Name { get; set; }

    public string? Description { get; set; }

    public required string? Author { get; set; }

    public required float? Price { get; set; }

    public string? ImagePath { get; set; }
}
