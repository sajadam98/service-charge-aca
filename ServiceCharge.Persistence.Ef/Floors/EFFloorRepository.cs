using ServiceCharge.Entities;
using ServiceCharge.Services.Floors.Contracts;

namespace ServiceCharge.Persistence.Ef.Floors;

public class EFFloorRepository(EfDataContext context) : FloorRepository
{
    public void Add(Floor floor)
    {
        context.Set<Floor>().Add(floor);
    }

    public bool IsFloorExistWithSameName(string name, int blockId)
    {
        return context.Set<Floor>().Any(f => f.Name == name && f.BlockId == blockId);
    }

    public Floor? FindByName(string name)
    {
        return context.Set<Floor>().FirstOrDefault(f => f.Name == name);
    }

    public void Delete(string floorName)
    {
        var floor = context.Set<Floor>().FirstOrDefault(f => f.Name == floorName);
        context.Set<Floor>().Remove(floor);
    }

    public bool IsExistById(int dtoFloorId)
    {
        return context.Set<Floor>().Any(f => f.Id == dtoFloorId);
    }
}