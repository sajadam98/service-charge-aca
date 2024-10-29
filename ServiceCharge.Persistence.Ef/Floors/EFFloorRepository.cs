using ServiceCharge.Entities;
using ServiceCharge.Services.Floors.Contracts;

namespace ServiceCharge.Persistence.Ef.Floors;

public class EFFloorRepository(EfDataContext context) : FloorRepository
{
    public void Add(Floor floor)
    {
        context.Set<Floor>().Add(floor);
    }

    public void Delete(Floor floor)
    {
        context.Set<Floor>().Remove(floor);
    }

    public Floor FindById(int floorId)
    {
       return context.Set<Floor>().FirstOrDefault(_ => _.Id == floorId);
    }

    public bool IsFloorExistWithSameName(string name, int blockId)
    {
        return context.Set<Floor>().Any(f => f.Name == name && f.BlockId == blockId);
    }

    public void Update(Floor floor)
    {
        context.Set<Floor>().Update(floor);
    }
}