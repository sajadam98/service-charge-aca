using ServiceCharge.Entities;

namespace ServiceCharge.TestTools.Floors;

public class FloorBuilder
{
    private readonly Floor _floor = new()
    {
        Name = "Floor"
    };

    public FloorBuilder WithUnitCount(int unitCount)
    {
        _floor.UnitCount = unitCount;
        return this;
    }

    public FloorBuilder WithBlockID(int BlockId)
    {
        _floor.BlockId = BlockId;
        return this;
    }

    public Floor Build()
    {
        return _floor;
    }
    
}