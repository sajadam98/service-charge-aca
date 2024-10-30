namespace ServiceCharge.Services.Unit;

public class UpdateUnitDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsActive { get; set; }
}