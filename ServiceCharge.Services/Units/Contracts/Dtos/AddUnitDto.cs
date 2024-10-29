namespace ServiceCharge.Services.Units.Contracts.Dtos;

public class AddUnitDto
{
    public required string Name { get; set; }
    public int FloorId { get; set; }
}