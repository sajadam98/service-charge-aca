namespace ServiceCharge.Application.Floors.Handlers.UpdateFloors.DTOs;

public class UpdateFloorWithUnitsCommand
{
    public required string Name { get; set; }
    public int UnitCount { get; set; }
    public List<int> UnitsId { get; set; } = [];
    public List<UpdateUnitOfFloorCommand> Units { get; set; } = [];
}