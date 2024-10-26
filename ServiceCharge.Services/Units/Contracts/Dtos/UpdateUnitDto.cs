namespace ServiceCharge.Services.Units.Contracts.Dtos;

public class UpdateUnitDto
{
    public required string Name { get; set; }
    public int FloorId { get; set; }
    public bool IsActive { get; set; }
}