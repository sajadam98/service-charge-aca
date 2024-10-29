using Microsoft.EntityFrameworkCore.Migrations.Operations;
using ServiceCharge.Entities;

namespace ServiceCharge.TestTools.Units;

public static class UnitFactory
{
    public static Unit Create(int floorId, string name = "dummy-unit")
    {
        return new Unit
        {
            Name = name,
            FloorId= floorId,
            IsActive = true,
        };
    }
    
    
}