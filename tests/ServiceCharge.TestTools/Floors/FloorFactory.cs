using ServiceCharge.Entities;
using ServiceCharge.Services.Floors.Contracts;

namespace ServiceCharge.TestTools.Floors;

public static class FloorFactory
{
    public static Floor Create(int blockId,string name="dummy_floor_name",int unitCount=1)
    {
        return new Floor()
        {
            Name=name,
            UnitCount = unitCount,
            BlockId = blockId
        };
    }
}