namespace ServiceCharge.Application.Floors.Handlers.UpdateFloors.Contracts.DTOs;

public class UpdateFloorWithUnitsCommand
{
    public required string Name { get; set; }
    public int UnitCount { get; set; }
    public bool IsActive { get; set; }
    public HashSet<UpdateUnitOfFloorCommand> Units { get; set; } = [];
}