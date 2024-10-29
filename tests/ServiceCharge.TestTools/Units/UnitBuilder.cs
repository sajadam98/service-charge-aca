using ServiceCharge.Entities;
using ServiceCharge.TestTools.Floors;

namespace ServiceCharge.TestTools.Units
{
    public class UnitBuilder
    {

        private readonly Unit _unit = new()
        {
            Name = "Dummy_Block_Name",
            IsActive = true,
            FloorId = 1

        };
        public UnitBuilder WithFloorId(int floorId)
        {
            _unit.FloorId = floorId;
            return this;
        }
        public UnitBuilder WithIsActive(bool isActive)
        {
            _unit.IsActive = isActive;
            return this;
        }
        public UnitBuilder WithName(string name)
        {
            _unit.Name = name;
            return this;
        }
        public Unit Build()
        {
            return _unit;
        }

    }
}