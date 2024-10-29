using Microsoft.EntityFrameworkCore;
using ServiceCharge.Entities;
using ServiceCharge.Services.Blocks.Contracts;
using ServiceCharge.Services.Floors.Contracts.Dto;

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

    public GetBlockFloorCapacityAndFloorCountDto? FindById(int blockId)
    {
        return context.Set<Block>().Where(b => b.Id == blockId)
            .Select(b => new GetBlockFloorCapacityAndFloorCountDto
            {
                FloorCapacity = b.FloorCount,
                FloorCount = b.Floors.Count
            }).FirstOrDefault();
    }
}