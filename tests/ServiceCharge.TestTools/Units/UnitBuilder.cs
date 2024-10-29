using ServiceCharge.Entities;

namespace ServiceCharge.TestTools.Units;

public class UnitBuilder(int floorId)
{
    private Unit _unit = new Unit
    {
        Name = "Name",
        IsActive = true,
        FloorId = floorId
    };

    public UnitBuilder WithId(int id)
    {
        _unit.Id = id;
        return this;
    }

    public UnitBuilder WithName(string name)
    {
        _unit.Name = name;
        return this;
    }

    public UnitBuilder WithIsActive(bool isActive)
    {
        _unit.IsActive = isActive;
        return this;
    }

    public Unit Build()
    {
        return _unit;
    }
}