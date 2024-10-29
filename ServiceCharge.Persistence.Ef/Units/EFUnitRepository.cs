using ServiceCharge.Entities;
using ServiceCharge.Persistence.Ef;
using ServiceCharge.Services.Units.Contracts;

namespace ServiceCharge.Persistence.Ef.Units
{
    public class EFUnitRepository(EfDataContext context) : UnitRepository
    {
        public void Add(Unit unit)
        {
            context.Set<Unit>().Add(unit);
        }

        public void Delete(Unit unit)
        {
            context.Set<Unit>().Remove(unit);
        }

        public Unit FindById(int unitId)
        {
            return context.Set<Unit>().FirstOrDefault(_ => _.Id == unitId);
        }

        public void Update(Unit unit)
        {
            context.Set<Unit>().Update(unit);
        }

    }
}