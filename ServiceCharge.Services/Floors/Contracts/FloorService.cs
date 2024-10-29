using ServiceCharge.Entities;
using AddFloorDto = ServiceCharge.Services.Floors.Contracts.Dto.AddFloorDto;

namespace ServiceCharge.Services.Floors.Contracts;

public interface FloorService
{
    int Add(int blockId, AddFloorDto dto);
    void Update(int blockId, string name,string updatename);
    void Delete(int blockId, string floorName);
}