using ServiceCharge.Entities;

namespace ServiceCharge.TestTools.Floors;

public static class FloorFactory
{
    public static Floor CreateFloor(
        int blockId,
        string floorName = "Dummy_Floor_Name",
        int unitCount = 2)
    {
        return new Floor()
        {
            BlockId = blockId,
            Name = floorName,
            UnitCount = unitCount
        };
    }
}