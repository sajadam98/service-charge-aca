namespace ServiceCharge.Persistence.Ef.Units;

public class EFUnitRepository(EfDataContext context) : UnitRepository
{
    public void Add(Unit unit)
    {
        context.Set<Unit>().Add(unit);
    }

    public bool IsDuplicate(string name)
    {
        return context.Set<Unit>().Any(_ => _.Name == name);
    }

    public Unit Find(int id)
    {
        return context.Set<Unit>().FirstOrDefault(_ => _.Id == id);
    }

    public void Update(Unit unit)
    {
        context.Set<Unit>().Update(unit);
    }

    public void Remove(Unit unit)
    {
        context.Set<Unit>().Remove(unit);
    }

    public void AddRange(List<Unit> units)
    {
        context.Set<Unit>().AddRange(units);
    }
}