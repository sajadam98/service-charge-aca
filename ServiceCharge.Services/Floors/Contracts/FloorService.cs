using ServiceCharge.Services.Floors.Contracts.Dto;
using AddFloorDto = ServiceCharge.Services.Floors.Contracts.Dto.AddFloorDto;

namespace ServiceCharge.Services.Floors.Contracts;

public interface FloorService
{
    int Add(int blockId, AddFloorDto dto);
    void Update(UpdateFloorDto updateDto, int floorId);
}