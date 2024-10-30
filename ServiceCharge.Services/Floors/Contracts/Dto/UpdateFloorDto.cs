namespace ServiceCharge.Services.Floors.Contracts;

public class UpdateFloorDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int UnitCount { get; set; }
}