namespace ServiceCharge.Service.Unit.Tests.Floors;

public static class FloorFactory
{
    public static Floor Crerate(
        int blockId,
        int unitCount = 1,
        string name = "name")
    {
        return new Floor()
        {
            Name = name,
            UnitCount = unitCount,
            BlockId = blockId
        };
    }
}