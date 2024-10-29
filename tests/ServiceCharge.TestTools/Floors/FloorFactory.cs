using ServiceCharge.Entities;
using ServiceCharge.Services.Floors.Contracts.Dto;

namespace ServiceCharge.TestTools.Floors;

public class FloorFactory
{
    public static Floor Create(int blockId ,string name =  "floor1",int unitCount = 1)
    {
        return new Floor()
        {
            Name = name,
            BlockId = blockId,
            UnitCount = unitCount,
        };
    }

   
}