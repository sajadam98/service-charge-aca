namespace ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts.DTOs;

public class AddUnitOfFloorCommand
{
    public required string Name { get; set; }
    public bool IsActive { get; set; }
}