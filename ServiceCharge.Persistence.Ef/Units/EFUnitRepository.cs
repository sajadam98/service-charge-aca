using ServiceCharge.Entities;
using ServiceCharge.Services.Units.Contracts;

namespace ServiceCharge.Persistence.Ef.Units;

public class EFUnitRepository(EfDataContext context) : UnitRepository
{
    public void Add(Unit unit)
    {
        context.Set<Unit>().Add(unit);
    }

    public Unit? FindById(int unitId)
    {
        return context.Set<Unit>().FirstOrDefault(u => u.Id == unitId);
    }

    public void Update(Unit unit)
    {
        context.Set<Unit>().Update(unit);
    }

    public void Delete(Unit unit)
    {
        context.Set<Unit>().Remove(unit);
    }
}