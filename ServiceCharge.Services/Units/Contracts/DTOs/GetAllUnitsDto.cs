namespace ServiceCharge.Services.Units.Contracts;

public class GetAllUnitsDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int FloorId { get; set; }
    public bool IsActive { get; set; }
}