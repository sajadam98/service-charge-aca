namespace ServiceCharge.Services.Floors.Contracts;

public class PutFloorDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int UnitCount { get; set; }
}