using ServiceCharge.Entities;

namespace ServiceCharge.Services.Floors.Contracts;

public interface FloorRepository
{
    void Add(Floor floor);
    bool IsFloorExistWithSameName(string name, int blockId);
    bool IsDuplicate(string dtoName, int blockId);
    Floor? Find(int floorId);
    bool IsDuplicateWithoutFloor(string name, int id, int blockId);
}