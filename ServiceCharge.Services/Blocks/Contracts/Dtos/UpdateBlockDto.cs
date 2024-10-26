namespace ServiceCharge.Services.Blocks.Contracts.Dtos;

public class UpdateBlockDto
{
    [Range(1, 100)]
    public int FloorCount { get; set; }
    public required string Name { get; set; }
}