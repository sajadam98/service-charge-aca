using ServiceCharge.Entities;
using ServiceCharge.Services.Unit;

namespace ServiceCharge.Persistence.Ef.Units;

public class EfUnitRepository(EfDataContext context):UnitRepository
{
    public void Add(Unit unit)
    {
        context.Set<Unit>().Add(unit);
    }
}