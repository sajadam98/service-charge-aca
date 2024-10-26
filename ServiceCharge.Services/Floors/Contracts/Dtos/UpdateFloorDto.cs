namespace ServiceCharge.Services.Floors.Contracts.Dtos;

public class UpdateFloorDto
{
    public required string Name { get; set; }
    public int UnitCount { get; set; }
}