using Microsoft.EntityFrameworkCore;
using ServiceCharge.Entities;
using ServiceCharge.Services.Blocks.Contracts;

namespace ServiceCharge.Persistence.Ef.Blocks;

public class EFBlockRepository(EfDataContext context) : BlockRepository
{
    public void Add(Block block)
    {
        context.Set<Block>().Add(block);
    }

    public bool IsDuplicate(string name)
    {
        return context.Set<Block>()
            .Any(_ => _.Name == name);
    }

    public bool IsExistById(int blockId)
    {
        return context.Set<Block>().Any(b => b.Id == blockId);
    }

    public Block? FindById(int id)
    {
        return context.Set<Block>().Include(_ => _.Floors)
            .SingleOrDefault(_ => _.Id == id);
    }

    public GetBlockInformationDto GetBlockInformationById(int id)
    {
        return context.Set<Block>().Where(_ => _.Id == id)
            .Select(_ => new GetBlockInformationDto()
            {
                Name = _.Name,
                FloorCapacity = _.FloorCount,
                FloorCount = _.Floors.Count
            }).FirstOrDefault();
    }
}