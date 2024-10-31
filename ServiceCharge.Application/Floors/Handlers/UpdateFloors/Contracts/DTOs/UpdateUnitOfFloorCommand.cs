namespace ServiceCharge.Application.Floors.Handlers.UpdateFloors.Contracts.DTOs;

public class UpdateUnitOfFloorCommand
{
    public required string Name { get; set; }
    public int Id { get; set; }
    public bool IsActive { get; set; }
}