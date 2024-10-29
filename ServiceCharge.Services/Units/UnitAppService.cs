using ServiceCharge.Entities;
using ServiceCharge.Services.Floors;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.Floors.Exceptions;
using ServiceCharge.Services.UnitOfWorks;
using ServiceCharge.Services.Units.Contracts;
using ServiceCharge.Services.Units.Exceptions;

namespace ServiceCharge.Services.Units;

public class UnitAppService(
    UnitRepository unitRepository,
    FloorRepository floorRepository,
    UnitOfWork unitOfWork) : UnitService
{
    public int Add(int floorId, AddUnitDto dto)
    {
        var floorDto = floorRepository.FindByIdWithExistingUnitsCount(floorId)
                       ?? throw new FloorNotFoundException();

        if (floorDto.ExistingUnitsCount >= floorDto.Floor.UnitCount)
        {
            throw new FloorUnitsCountException();
        }

        var unit = new Unit
        {
            Name = dto.Name,
            IsActive = dto.IsActive,
            FloorId = floorId,
        };

        unitRepository.Add(unit);
        unitOfWork.Save();
        return unit.Id;
    }

    public void Update(int unitId, UpdateUnitDto dto)
    {
        var unit = unitRepository.FindById(unitId)
                   ?? throw new UnitNotFoundException();

        unit.Name = dto.Name;
        unitRepository.Update(unit);
        unitOfWork.Save();
    }

    public void Delete(int unitId)
    {
        var unit = unitRepository.FindById(unitId)
            ?? throw new UnitNotFoundException();
        
        unitRepository.Delete(unit);
        unitOfWork.Save();
    }
}