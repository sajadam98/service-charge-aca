using ServiceCharge.Services.Units.Contracts.DTOs;

namespace ServiceCharge.Services.Units.Contracts;

public interface UnitService
{
    void Add(AddUnitDto unitDto);
    void Delete(int unit1Id);
    void Update(int unitId, UpdateUnitDto updateDto);
}