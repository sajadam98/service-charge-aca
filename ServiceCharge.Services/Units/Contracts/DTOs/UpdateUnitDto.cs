namespace ServiceCharge.Services.Units.Contracts.DTOs;

public class UpdateUnitDto
{
    public required string Name { get; set; }
    public bool IsActive { get; set; }
}