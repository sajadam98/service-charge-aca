namespace ServiceCharge.Services.Floors;

public class GetFloorDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int UnitCount { get; set; }
    public int BlockId { get; set; }
}