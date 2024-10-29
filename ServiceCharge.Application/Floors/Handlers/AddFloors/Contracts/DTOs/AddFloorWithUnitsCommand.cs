namespace ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts.DTOs;

public class AddFloorWithUnitsCommand
{
    public required string Name { get; set; }
    public int UnitCount { get; set; }
    public HashSet<AddUnitOfFloorCommand> Units { get; set; } = [];
}