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

    public Floor? FindById(int floorId)
    {
        return context.Set<Floor>().FirstOrDefault(_=>_.Id == floorId);
    }

    public bool IsFloorExistWithSameNameButNotPreviousName(string dtoName, int floorId, int blockId)
    {
        return context.Set<Floor>()
            .Where(_=>_.BlockId == blockId)
            .Any(f => f.Name == dtoName && f.Id != floorId);
    }

    public void Delete(Floor floor)
    {
        context.Set<Floor>().Remove(floor);
    }

    public Floor? Find(int unitDtoFloorId)
    {
        return context.Set<Floor>().FirstOrDefault(f => f.Id == unitDtoFloorId);
    }
}