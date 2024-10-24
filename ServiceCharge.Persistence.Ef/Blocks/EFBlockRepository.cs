

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

    public Block? FindById(int id)
    {
        return context.Set<Block>().SingleOrDefault(_ => _.Id == id);
    }

    public bool IsNameDuplicate(string name)
    {
        return context.Set<Block>().Any(_ => _.Name == name);
    }

    public bool IsExistById(int id)
    {
        return context.Set<Block>().Any(_ => _.Id == id);
    }
}