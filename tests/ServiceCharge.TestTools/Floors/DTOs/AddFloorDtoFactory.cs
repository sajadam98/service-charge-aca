using ServiceCharge.Services.Floors.Contracts.Dto;

namespace ServiceCharge.TestTools.Floors.DTOs;

public static class AddFloorDtoFactory
{
    public static AddFloorDto CreateDto(string floorName = "Dummy_Floor_Name",
        int unitCount = 2)
    {
        return new AddFloorDto()
        {
            Name = floorName,
            UnitCount = unitCount
        };
    }
}