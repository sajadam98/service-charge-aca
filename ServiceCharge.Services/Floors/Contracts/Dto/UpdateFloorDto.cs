namespace ServiceCharge.Services.Floors.Contracts;

public class UpdateFloorDto
{
    public int UnitCount { get; set; }
    public required string Name { get; set; }
    
}