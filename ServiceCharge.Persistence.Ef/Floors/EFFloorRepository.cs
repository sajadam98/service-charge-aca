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

    public Floor? FindById(int id)
    {
        return context.Set<Floor>().SingleOrDefault(_ => _.Id == id);
    }

    public bool IsExistInBlockByName(string name)
    {
        return context.Set<Floor>().Any(_ => _.Name == name);
    }

    public GetFloorInfoDto? GetFloorInfoById(int id)
    {
        var query = (
                from floor in context.Set<Floor>()
                where floor.Id == id
                join unit in context.Set<Unit>()
                    on floor.Id equals unit.FloorId
                    into UnitGroup
                from unitGrp in UnitGroup.DefaultIfEmpty()
                select new
                {
                    Id = floor.Id,
                    Name = floor.Name,
                    UnitCapaciry = floor.UnitCount,
                    BlockId = floor.BlockId,
                    Unit = unitGrp == null ? 0 : 1
                })
            .GroupBy(_ => _.Id)
            .Select(_ => new GetFloorInfoDto()
            {
                Id = _.Key,
                Name = _.First().Name,
                UnitCapacity = _.First().UnitCapaciry,
                BlockId = _.First().BlockId,
                UnitCount = _.Sum(_=>_.Unit)
                // UnitCount = _.Count(f => f != null)   <====!!!
            });

        return query.SingleOrDefault();
    }

    public void Delete(Floor floor)
    {
        context.Set<Floor>().Remove(floor);
    }
}