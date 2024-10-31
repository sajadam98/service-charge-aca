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

    public List<Unit> FindByIds(int floorId, List<int> unitIds)
    {
        return context.Set<Unit>()
            .Where(u => unitIds.Contains(u.Id) && u.FloorId == floorId)
            .ToList();
    }

    public void UpdateRange(List<Unit> units)
    {
        context.Set<Unit>().UpdateRange(units);
    }
}