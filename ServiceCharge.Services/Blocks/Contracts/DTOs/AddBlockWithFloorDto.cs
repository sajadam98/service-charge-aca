using ServiceCharge.Services.Floors.Contracts.Dto;

namespace ServiceCharge.Services.Blocks.Contracts.DTOs;

public class AddBlockWithFloorDto
{
    public List<AddFloorDto> Floors { get; set; } = [];
    public required string Name { get; set; }
}