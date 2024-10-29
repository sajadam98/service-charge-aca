using ServiceCharge.Entities;

namespace ServiceCharge.Services.Floors.Contracts;

public interface FloorRepository
{
    void Add(Floor floor);
    void Delete(Floor floor);
    Floor FindById(int floorId);
    bool IsFloorExistWithSameName(string name, int blockId);
    void Update(Floor floor);
}