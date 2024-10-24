using System.ComponentModel.DataAnnotations;

namespace ServiceCharge.Services.Blocks.Contracts;

public class AddFloorDto
{
    public required string Name { get; set; }
    [Range(1,100)] public int UnitCount { get; set; }
}