namespace ServiceCharge.Persistence.Ef.Floors;

public class EFFloorRepository(EfDataContext context) : FloorRepository
{
    public void Add(Floor floor)
    {
        context.Set<Floor>().Add(floor);
    }
    public bool IsDuplicate(string name)
    {
        return context.Set<Floor>().Any(_ => _.Name == name);
    }

    public Floor Find(int id)
    {
        return context.Set<Floor>().FirstOrDefault(_=>_.Id == id);
    }

    public int UnitsCount(int id)
    {
        return context.Set<Unit>().Count(_=>_.FloorId == id);
    }

    public void Update(Floor floor)
    {
        context.Set<Floor>().Update(floor);
    }

    public void Remove(Floor floor)
    {
        context.Set<Floor>().Remove(floor);
    }
}