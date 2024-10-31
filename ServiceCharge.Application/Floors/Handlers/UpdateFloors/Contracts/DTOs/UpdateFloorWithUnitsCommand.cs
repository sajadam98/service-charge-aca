namespace ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts.DTOs;

public class UpdateFloorWithUnitsCommand
{
    public required string Name { get; set; }
    public int UnitCount { get; set; }
    public HashSet<UpdateUnitOfFloorCommand> Units { get; set; } = [];
}