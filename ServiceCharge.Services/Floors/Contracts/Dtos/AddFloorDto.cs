namespace ServiceCharge.Services.Floors.Contracts.Dtos;

public class AddFloorDto
{
    public required string Name { get; set; }
    public int UnitCount { get; set; }
}