namespace ServiceCharge.Services.Floors.Contracts;

public class GetUnitDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int FloorId { get; set; }
    public bool IsActive { get; set; }
}