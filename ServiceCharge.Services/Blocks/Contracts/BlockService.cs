using System.ComponentModel.DataAnnotations;

namespace ServiceCharge.Services.Blocks.Contracts;

public interface BlockService
{
    void Add(AddBlockDto dto);
    void AddWithFloor(AddBlockWithFloorDto dto);
}

public class AddBlockDto
{
    [Range(1, 100)] public int FloorCount { get; set; }
    public required string Name { get; set; }
}

public class AddBlockWithFloorDto
{
    public List<AddFloorDto> Floors { get; set; } = [];
    public required string Name { get; set; }
}

public class AddFloorDto
{
    public string Name { get; set; }
    public int UnitCount { get; set; }
}