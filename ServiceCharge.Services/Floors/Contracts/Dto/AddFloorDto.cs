namespace ServiceCharge.Services.Floors.Contracts.Dto;

public class AddFloorDto
{
    public required string Name { get; set; }
    public int UnitCount { get; set; }
}