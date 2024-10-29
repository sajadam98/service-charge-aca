using Microsoft.EntityFrameworkCore;
using ServiceCharge.Entities;
using ServiceCharge.Services.Blocks.Contracts;
using ServiceCharge.Services.Floors.Contracts.Dtos;

namespace ServiceCharge.Persistence.Ef.Blocks;

public class EfBlockQuery : BlockQuery
{
    private readonly EfDataContext _context;

    public EfBlockQuery(EfDataContext context)
    {
        _context = context;
    }

    public HashSet<GetBlockDto> GetAll()
    {
        return _context.Set<Block>()
            .Select(_ =>
                new GetBlockDto()
                {
                    Id = _.Id,
                    Name = _.Name,
                    CreationDate = _.CreationDate,
                    FloorCount = _.FloorCount,
                }).ToHashSet();
    }

    public HashSet<GetBlockWithFloorsDto> GetAllWithFloors()
    {
        var query = _context.Set<Block>()
            .Include(_ => _.Floors)
            .Select(_ => new GetBlockWithFloorsDto()
            {
                Id = _.Id,
                Name = _.Name,
                CreationDate = _.CreationDate,
                FloorCount = _.FloorCount,
                Floors = _.Floors.Select(_ => new GetFloorDto()
                {
                    Id = _.Id,
                    Name = _.Name,
                    UnitCount = _.UnitCount,
                    BlockId = _.BlockId,
                }).ToHashSet()
            });
        return query.ToHashSet();
    }

    public HashSet<GetAllWithAddedFloorCountDto> GetAllWithAddedFloorCount()
    {
        var query = _context.Set<Block>()
            .Select(_ => new GetAllWithAddedFloorCountDto()
            {
                Id = _.Id,
                Name = _.Name,
                FloorCount = _.FloorCount,
                AddedFloorCount = _.Floors.Count
            });
        return query.ToHashSet();
    }
}