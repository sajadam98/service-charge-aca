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
    public bool IsDuplicate(string dtoName, int blockId)
    {
        return context.Set<Floor>()
            .Any(floor => floor.BlockId == blockId && floor.Name == dtoName);
    }

    public Floor? Find(int floorId)
    {
        return context.Set<Floor>().Find(floorId);
    }

    public bool IsDuplicateWithoutFloor(string updateDtoName, int floorId, int floorBlockId)
    {
        return context.Set<Floor>().Where(_ => _.BlockId == floorBlockId)
            .Any(_ => _.Name == updateDtoName && _.Id != floorId);
    }
}