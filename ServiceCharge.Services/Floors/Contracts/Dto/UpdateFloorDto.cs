using System.Reflection.Metadata.Ecma335;

namespace ServiceCharge.Services.Floors.Contracts.Dto;

public class UpdateFloorDto
{
    public required string Name { get; set; }
    public int UnitCount { get; set; }
}