namespace ServiceCharge.Application.Floors.Handler.UpdateFloorWithUnits.Contracts;

public class UpdateUnitCommand
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsActive { get; set; }
}