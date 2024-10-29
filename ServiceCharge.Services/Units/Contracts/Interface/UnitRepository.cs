using ServiceCharge.Entities;

namespace ServiceCharge.Services.Units.Contracts.Interface;

public interface UnitRepository
{
    void Add(Unit unit);
    int CountRegisteredUnit(int floorId);
    Unit? Find(int unitId);
    void Delete(Unit unit);
    void AddRange(List<Unit> units);
}