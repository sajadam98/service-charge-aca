namespace ServiceCharge.Services.Floors.Contracts;

public class GetFloorInfoDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int UnitCount { get; set; }
    public int UnitCapacity { get; set; }
    public int BlockId { get; set; }
}