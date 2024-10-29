using ServiceCharge.Entities;

namespace ServiceCharge.Services.Floors.Contracts;

public interface FloorRepository
{
    void Add(Floor floor);
    bool IsFloorExistWithSameName(string name, int blockId);
    Floor? FindById(int id);
    bool IsExistInBlockByName(string name);
    GetFloorInfoDto? GetFloorInfoById(int id);
    void Delete(Floor floor);
}