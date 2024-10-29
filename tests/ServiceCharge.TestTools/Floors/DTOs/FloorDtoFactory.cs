using ServiceCharge.Services.Floors.Contracts.Dto;

namespace ServiceCharge.TestTools.Floors.DTOs;

public class FloorDtoFactory
{
    public static AddFloorDto AddFloorDto(int unitCount, string name = "floor1")
    {
        return new AddFloorDto()
        {
            Name = name,
            UnitCount = unitCount,
        };
    }


    public static UpdateFloorDto UpdateFloorDto(string name, int unitCount)
    {
        return new UpdateFloorDto()
        {
            Name = name,
            UnitCount = unitCount,
        };
    }

 
}