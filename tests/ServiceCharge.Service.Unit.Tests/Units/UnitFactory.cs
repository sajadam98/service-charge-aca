namespace ServiceCharge.Service.Unit.Tests.Units;

public static class UnitFactory
{
    public static Entities.Unit Create(
        int floorId,
        string name = "name",
        bool isActive = true)
    {
        return new Entities.Unit()
        {
            FloorId = floorId,
            Name = name,
            IsActive = isActive
        };
    }
}