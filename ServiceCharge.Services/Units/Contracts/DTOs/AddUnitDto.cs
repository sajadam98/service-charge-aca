namespace ServiceCharge.Services.Units.Contracts.DTOs;

public class AddUnitDto
{
    public required string Name { get; set; }
    public required int FloorId{ get; set; }
}