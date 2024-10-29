namespace ServiceCharge.Services.Units.Contracts;

public class GetAllUnitsWithFloorAndBlockNameDto
{
    public int Id { get; set; }
    public String Name { get; set; }
    public int FloorId { get; set; }
    public bool IsActive { get; set; }
    public string FloorName { get; set; }
    public string BlockName { get; set; }
}