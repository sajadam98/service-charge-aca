namespace ServiceCharge.Service.Unit.Tests.Blocks.FactoryBuilder;

public class BlockBuilder
{
    private readonly Block _block;
    public BlockBuilder()
    {
        _block = new Block
        {
            Name = "name",
            CreationDate = new DateTime(2021,1,1),
            FloorCount = 1
        }; 
    }
    public BlockBuilder WithName(string name)
    {
        _block.Name = name;
        return this;
    }
    public BlockBuilder WithFloorCount(int floorCount)
    {
        _block.FloorCount = floorCount;
        return this;
    }
    public Block Build()
    {
        return _block;
    }
}