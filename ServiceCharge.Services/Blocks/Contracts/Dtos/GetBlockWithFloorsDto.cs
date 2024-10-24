using ServiceCharge.Services.Floors.Contracts.Dtos;

namespace ServiceCharge.Services.Blocks.Contracts;

public class GetBlockWithFloorsDto
{
    public int Id { get; set; }
    public int FloorCount { get; set; }
    public required string Name { get; set; }
    public DateTime CreationDate { get; set; }
    public HashSet<GetFloorDto> Floors { get; set; } = [];
}