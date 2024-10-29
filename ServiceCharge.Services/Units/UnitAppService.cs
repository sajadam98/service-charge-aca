using ServiceCharge.Entities;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.Floors.Exceptions;
using ServiceCharge.Services.UnitOfWorks;
using ServiceCharge.Services.Units.Contracts;
using ServiceCharge.Services.Units.Contracts.DTOs;
using ServiceCharge.Services.Units.Exceptions;

namespace ServiceCharge.Services.Units;

public class UnitAppService(UnitRepository unitRepository,
    UnitOfWork unitOfWork,
    FloorRepository floorRepository):UnitService
{
    public void Add(AddUnitDto unitDto)
    {
        var floor = floorRepository.Find(unitDto.FloorId);

        if (floor == null)
        {
            throw new FloorNotFoundException();
        }

        if (!ValidateAddUnit(floor))
        {
            throw new MaxUnitCountException();
        }

        var unit = new Unit()
        {
            Name = unitDto.Name,
            FloorId = unitDto.FloorId,
            IsActive = true
        };

        unitRepository.Add(unit);

        unitOfWork.Save();
    }

    public void Delete(int unitId)
    {
        var unit = unitRepository.Find(unitId);

        if (unit == null)
        {
            throw new UnitNotFoundException();
        }

        unitRepository.Delete(unit!);

        unitOfWork.Save();
    }

    public void Update(int unitId, UpdateUnitDto updateDto)
    {
        var unit = unitRepository.Find(unitId);
        if (unit == null)
        {
            throw new UnitNotFoundException();
        }

        if (unitRepository.IsDuplicateWithSameName(updateDto.Name, unit.FloorId))
        {
            throw new DuplicateUnitNameException();
        }
        
        unit.Name = updateDto.Name;
        unit.IsActive = updateDto.IsActive;
        
        unitOfWork.Save();
    }

    public void AddRange(List<AddUnitDto> unitsDto)
    {
        var units = new List<Unit>();
        units = units.Select(unit => new Unit()
        {
            Name = unit.Name,
            FloorId = unit.FloorId,
            IsActive = true
        }).ToList();

        unitRepository.AddRange(units);
        unitOfWork.Save();
    }

    private bool ValidateAddUnit(Floor floor)
    {
        int registeredUnit = unitRepository.CountRegisteredUnit(floor.Id);

        if (!(floor.UnitCount > registeredUnit))
            return false;
        return true;
    }
}