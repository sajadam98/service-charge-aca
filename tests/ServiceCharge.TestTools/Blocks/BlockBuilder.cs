using ServiceCharge.Entities;

namespace ServiceCharge.TestTools.Blocks;

public class BlockBuilder
{
    private readonly Block _block = new()
    {
        Name = "Dummy_Block_Name",
        CreationDate = DateTime.UtcNow,
        FloorCount = 1
    };

    public BlockBuilder WithFloorCount(int value)
    {
        _block.FloorCount = value;
        return this;
    }

    public BlockBuilder WithName(string name)
    {
        _block.Name = name;
        return this;
    }

    public Block Build()
    {
        return _block;
    }
}