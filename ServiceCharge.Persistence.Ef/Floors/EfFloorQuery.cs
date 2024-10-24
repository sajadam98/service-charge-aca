using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.Floors.Contracts.Dtos;

namespace ServiceCharge.Persistence.Ef.Floors;

public class EfFloorQuery : FloorQuery
{
    private readonly EfDataContext _context;

    public EfFloorQuery(EfDataContext context)
    {
        _context = context;
    }

    public HashSet<GetFloorDto> GetAll()
    {
        var query = _context.Set<Floor>()
            .Select(_ => new GetFloorDto()
            {
                Id = _.Id,
                Name = _.Name,
                UnitCount = _.UnitCount,
                BlockId = _.BlockId
            });
        return query.ToHashSet();
    }

    public HashSet<GetFloorWithUnitsDto> GetAllWithUnits()
    {
        var query =
            (from floor in _context.Set<Floor>()
                join unit in _context.Set<Unit>()
                    on floor.Id equals unit.FloorId
                select new
                {
                    Id = floor.Id,
                    Name = floor.Name,
                    UnitCount = floor.UnitCount,
                    BlockId = floor.BlockId,
                    Unit = unit
                })
            .GroupBy(_ => _.Id)
            .Select(_ => new GetFloorWithUnitsDto()
            {
                Id = _.Key,
                Name = _.First().Name,
                UnitCount = _.First().UnitCount,
                BlockId = _.First().BlockId,
                Units = _.Select(_ => _.Unit).Select(_ => new GetUnitDto()
                {
                    Id = _.Id,
                    Name = _.Name,
                    IsActive = _.IsActive,
                    FloorId = _.FloorId
                }).ToHashSet()
            });

        return query.ToHashSet();
    }
}