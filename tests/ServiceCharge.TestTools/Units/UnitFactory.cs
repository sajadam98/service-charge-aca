using ServiceCharge.Entities;

namespace ServiceCharge.TestTools.Units;

public static class UnitFactory
{
    public static Unit Create(
        int floorId,
        string name = "dummy_unit_name",
        bool isActive = true)
    {
        return new Unit()
        {
            FloorId = floorId,
            Name = name,
            IsActive = isActive
        };
    }
}