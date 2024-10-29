using ServiceCharge.Entities;

namespace ServiceCharge.TestTools.Blocks;

public static class BlockFactory
{
    public static Block Create(
        string name = "dummy_block_name",
        int floorCount = 1,
        DateTime? creationDate = null)
    {
        creationDate ??= DateTime.UtcNow;
        return new Block()
        {
            Name = name,
            FloorCount = floorCount,
            CreationDate = creationDate.Value
        };
    }
}