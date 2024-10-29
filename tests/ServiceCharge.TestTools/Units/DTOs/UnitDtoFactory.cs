using ServiceCharge.Services.Units.Contracts.DTOs;

namespace ServiceCharge.TestTools.Units.DTOs;

public static class UnitDtoFactory
{
    public static AddUnitDto CreateAddUnitDto(string name, int floorId)
    {
        var unitDto = new AddUnitDto()
        {
            Name = name,
            FloorId = floorId,
        };
        return unitDto;
    }

    public static UpdateUnitDto CreateUpdateUnitDto(string name, bool? isActive = true)
    {
        return new UpdateUnitDto
        {
            Name = name,
            IsActive = isActive ??= true
        };
    }
}