namespace ServiceCharge.Application.Floors.Handler.UpdateFloorWithUnits.Contracts;

public class UpdateFloorWithUnitsCommand
{
    public required string Name { get; set; }
    public int UnitCount { get; set; }
    public List<UpdateUnitCommand> Units { get; set; } = [];
}