using ServiceCharge.Entities;

namespace ServiceCharge.TestTools.Units;

public static class UnitFactory
{
    public static Unit Creat(int floorId = 1, string name = "dumyy",bool isActive = true)
    {
        return new Unit()
        {
            FloorId = floorId,
            Name = name,
            IsActive = isActive,
        };
        
    }
}