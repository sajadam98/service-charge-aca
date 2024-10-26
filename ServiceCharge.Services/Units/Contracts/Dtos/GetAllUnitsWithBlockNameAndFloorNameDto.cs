namespace ServiceCharge.Services.Units.Contracts.Dtos;

public class GetAllUnitsWithBlockNameAndFloorNameDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int FloorId { get; set; }
    public bool IsActive { get; set; }
    public string FloorName { get; set; }
    public string BlockName { get; set; }
}