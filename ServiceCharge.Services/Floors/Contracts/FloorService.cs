using AddFloorDto = ServiceCharge.Services.Floors.Contracts.Dto.AddFloorDto;

namespace ServiceCharge.Services.Floors.Contracts;

public interface FloorService
{
    int Add(int blockId, AddFloorDto dto);
    void Update(int floorId,UpdateFloorDto dto);
    void Delete(int floorId);
}

public class UpdateFloorDto
{
    public int UnitCount { get; set; }
    public required string Name { get; set; }
    
}