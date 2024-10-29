using ServiceCharge.Entities;

namespace ServiceCharge.Services.Floors.Contracts;

public interface FloorRepository
{
    void Add(Floor floor);
    bool IsFloorExistWithSameName(string name, int blockId);
    Floor? FindById(int floorId);
    bool IsFloorExistWithSameNameButNotPreviousName(string dtoName, int floorId, int blockId);
    void Delete(Floor floor);
    Floor? Find(int unitDtoFloorId);
}