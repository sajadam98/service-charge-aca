using ServiceCharge.Entities;
using ServiceCharge.Services.Units.Contracts.Dtos;

namespace ServiceCharge.Services.Units.Contracts.Interface;

public interface UnitService
{
    void Add(AddUnitDto unitDto);
    void Delete(int unitId);
    void AddRange(List<AddUnitDto> unitsDto);
}