using ServiceCharge.Entities;

namespace ServiceCharge.TestTools.Floors;

public class FloorBuilder(int blockId)
{
    private readonly Floor _floor = new Floor()
    {
        Name = "Dummy_Floor_Name",
        UnitCount = 2,
        BlockId = blockId
    };

    public FloorBuilder WithName(string name)
    {
        _floor.Name = name;
        return this;
    }

    public FloorBuilder WithUnitCount(int unitCount)
    {
        _floor.UnitCount = unitCount;
        return this;
    }

    public Floor Build()
    {
        return _floor;
    }
}