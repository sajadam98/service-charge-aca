namespace ServiceCharge.Services.Units.Contracts.Interfaces;

public interface UnitRepository
{
    void Add(Unit unit);
    bool IsDuplicate(string name);
    Unit Find(int id);
    void Update(Unit unit);
    void Remove(Unit unit);
    void AddRange(List<Unit> units);
    HashSet<Unit> GetAllByFloorId(int floorId);
}