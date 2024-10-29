using System.Reflection.Metadata.Ecma335;

namespace ServiceCharge.Application.Floors.Handlers.UpdateFloors.DTOs;

public class UpdateUnitOfFloorCommand
{
    public required string Name { get; set; }
    public bool IsActive { get; set; }
}