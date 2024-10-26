namespace ServiceCharge.Services.Blocks.Contracts.Dtos;

public class GetByIdAndFloorsInfo
{
    public int FloorCount { get; set; }
    public required string Name { get; set; }
    public DateTime CreationDate { get; set; }

    public List<GetAllFloorsDto> Floors { get; set; } = [];
}