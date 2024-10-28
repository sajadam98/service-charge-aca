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

    public Block? FindWithFloors(int dtoBlockId)
    {
        return context.Set<Block>().Include(_=>_.Floors)
            .FirstOrDefault(_=>_.Id == dtoBlockId);
    }   
}