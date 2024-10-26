namespace ServiceCharge.Persistence.Ef.Blocks;

public class EFBlockQuery(EfDataContext context) : BlockQuery
{
    public HashSet<GetAllBlocksDto> GetAll()
    {
        return context.Set<Block>().Select(_ => new GetAllBlocksDto()
        {
            Name = _.Name,
            CreationDate = _.CreationDate,
            FloorCount = _.FloorCount,
            Id = _.Id,
        }).ToHashSet();
    }
    public Block GetById(int id)
    {
        return context.Set<Block>().FirstOrDefault(_ => _.Id == id);
    }

    public GetByIdAndFloorsInfo GetByIdAndFloorsInfo(int id)
    {
        return context.Set<Block>().Where(_ => _.Id == id).Select(_ => new GetByIdAndFloorsInfo()
        {
            Name = _.Name,
            FloorCount = _.FloorCount,
            CreationDate = _.CreationDate,
            Floors = _.Floors.Any() ? _.Floors.Select(f => new GetAllFloorsDto()
                {
                    Id = f.Id,
                    Name = f.Name,
                    UnitCount = f.UnitCount,
                    BlockId = f.BlockId,
                }).ToList()
                : new List<GetAllFloorsDto>()
        }).SingleOrDefault();
    }
    public HashSet<GetAllBlocksWithFloorsDto> GetAllWithFloorsInfo()
    {
        return context.Set<Block>().Select(_ => new GetAllBlocksWithFloorsDto()
        {
            Name = _.Name,
            FloorCount = _.FloorCount,
            CreationDate = _.CreationDate,
            Id = _.Id,
            Floors = _.Floors.Any()
                ? _.Floors.Select(f => new GetAllFloorsDto()
                {
                    Id = f.Id,
                    Name = f.Name,
                    UnitCount = f.UnitCount,
                    BlockId = f.BlockId,
                }).ToList() : new List<GetAllFloorsDto>()
        }).ToHashSet();
    }
}