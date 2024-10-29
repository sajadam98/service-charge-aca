using ServiceCharge.Services.Floors.Contracts.Dto;
using AddFloorDto = ServiceCharge.Services.Floors.Contracts.Dto.AddFloorDto;

namespace ServiceCharge.Services.Floors.Contracts;

public interface FloorService
{
    int Add(int blockId, AddFloorDto dto);
    void Delete(int id);
    
    void Update(int floorId, UpdateUnitDto dto);
}