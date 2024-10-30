namespace ServiceCharge.Application.Floors.Handler.AddFloorWithUnits.Contracts;

public class AddFloorWithUnitsCommand
{
    public required string Name { get; set; }
    public int UnitCount { get; set; }
    public List<AddUnitCommand> Units { get; set; } = [];
}