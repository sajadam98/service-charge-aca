using ServiceCharge.Services.Units.Contracts;

namespace ServiceCharge.Persistence.Ef.Units;

public class EfUnitQuery : UnitQuery
{
    private readonly EfDataContext _context;

    public EfUnitQuery(EfDataContext context)
    {
        _context = context;
    }

    public HashSet<GetAllUnitsWithFloorAndBlockNameDto>
        GetAllUnitsWithFloorAndBlockName()
    {
        var query = (from unit in _context.Set<Unit>()
            join floor in _context.Set<Floor>()
                on unit.FloorId equals floor.Id
            join block in _context.Set<Block>()
                on floor.BlockId equals block.Id
            select new GetAllUnitsWithFloorAndBlockNameDto()
            {
                Id = unit.Id,
                Name = unit.Name,
                FloorName = floor.Name,
                BlockName = block.Name,
                FloorId = floor.Id,
                IsActive = unit.IsActive
            });
        return query.ToHashSet();
    }
}