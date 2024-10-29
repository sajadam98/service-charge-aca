using ServiceCharge.Entities;

namespace ServiceCharge.Services.Floors.Contracts;

public interface FloorRepository
{
    void Add(Floor floor);
    bool IsFloorExistWithSameName(string name, int blockId);
    Floor? FindByName(string name);
    void Delete(string floorName);
    bool IsExistById(int dtoFloorId);
}   