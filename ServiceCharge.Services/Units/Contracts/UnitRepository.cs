using ServiceCharge.Entities;

namespace ServiceCharge.Services.Units.Contracts;

public interface UnitRepository
{
    void Add(Unit unit);
    Unit? Find(int unitId);
    void Delete(Unit unit);
    void AddRange(List<Unit> units);
    int CountRegisteredUnit(int floorId);
    bool IsDuplicateWithSameName(string updateDtoName, int unitFloorId);
}