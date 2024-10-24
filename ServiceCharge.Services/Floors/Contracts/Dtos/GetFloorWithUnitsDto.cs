namespace ServiceCharge.Services.Floors.Contracts;

public class GetFloorWithUnitsDto
{
    public int Id { get; set; }
    public int UnitCount { get; set; }
    public required string Name { get; set; }
    public int BlockId { get; set; }
    public HashSet<GetUnitDto> Units { get; set; }
}