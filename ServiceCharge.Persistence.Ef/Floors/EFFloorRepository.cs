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
        return context.Set<Floor>()
            .Any(f => f.Name == name && f.BlockId == blockId);
    }

    public FindByIdWithUnitsCountDto? FindByIdWithExistingUnitsCount(
        int floorId)
    {
        return (from floor in context.Set<Floor>()
            where floor.Id == floorId
            join units in context.Set<Unit>()
                on floor.Id equals units.FloorId
                into floorsUnit
            select new FindByIdWithUnitsCountDto
            {
                Floor = new Floor
                {
                    Id = floor.Id,
                    Name = floor.Name,
                    UnitCount = floor.UnitCount,
                    BlockId = floor.BlockId
                },
                ExistingUnitsCount = floorsUnit.Count()
            }).FirstOrDefault();
    }

    public void Update(Floor floor)
    {
        context.Set<Floor>().Update(floor);
    }

    public void Delete(Floor floor)
    {
        context.Set<Floor>().Remove(floor);
    }
}