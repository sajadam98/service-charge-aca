namespace ServiceCharge.Services.Units.Contracts;

public class AddUnitDto
{
    public required string Name { get; set; }
    public bool IsActive { get; set; }
}