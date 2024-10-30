namespace ServiceCharge.Application.Floors.Handler.AddFloorWithUnits.Contracts;

public class AddUnitCommand
{
    public required string Name { get; set; }
    public bool IsActive { get; set; }
}