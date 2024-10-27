using System.ComponentModel.DataAnnotations;

namespace ServiceCharge.Services.Blocks.Contracts.DTOs;

public class AddBlockDto
{
    [Range(1, 100)] public int FloorCount { get; set; }
    public required string Name { get; set; }
}