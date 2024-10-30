﻿namespace ServiceCharge.Services.Unit;

public interface UnitService
{
    int Add(int floorId, AddUnitDto dto);
    void AddRange(int floorId,List<AddUnitDto> unitDtos);
    void UpdateRange(int floorId, List<UpdateUnitDto> unitDtos);
}