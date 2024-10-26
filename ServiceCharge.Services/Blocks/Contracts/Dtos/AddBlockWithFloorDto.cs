namespace ServiceCharge.Services.Blocks.Contracts.Dtos;

public class AddBlockWithFloorDto
{
    public List<AddFloorDto> Floors { get; set; } = [];
    public required string Name { get; set; }
}