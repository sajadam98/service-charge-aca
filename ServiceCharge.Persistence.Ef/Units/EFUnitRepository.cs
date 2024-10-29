using ServiceCharge.Entities;
using ServiceCharge.Services.Units.Contracts.Interface;

namespace ServiceCharge.Persistence.Ef.Units;

public class EFUnitRepository(EfDataContext context):UnitRepository
{
    public void Add(Unit unit)
    {
        context.Set<Unit>().Add(unit);
    }

    public int CountRegisteredUnit(int floorId)
    {
       return context.Set<Unit>().Count(_=>_.FloorId == floorId);
    }

    public Unit? Find(int unitId)
    {
        return context.Set<Unit>().Find(unitId);
    }

    public void Delete(Unit unit)
    {
        context.Set<Unit>().Remove(unit);
    }

    public void AddRange(List<Unit> units)
    {
        context.Set<Unit>().AddRange(units);
    }
}