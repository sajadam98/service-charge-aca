namespace ServiceCharge.Services.Units;

public class UnitAppService(
    UnitRepository unitRepository,
    UnitOfWork unitOfWork,
    FloorRepository floorRepository) : UnitService
{
    public int Add(int id, AddUnitDto dto)
    {
        var isDuplicate = unitRepository.IsDuplicate(dto.Name);
        if (isDuplicate)
            throw new UnitNameDuplicateException();

        var floor = floorRepository.Find(id)
                    ?? throw new FloorNotFoundException();

        if (floorRepository.UnitsCount(id) >= floor.UnitCount)
            throw new FloorHasMaxUnitsCountException();

        var unit = new Unit()
        {
            Name = dto.Name,
            FloorId = id,
            IsActive = dto.IsActive,
        };

        unitRepository.Add(unit);
        unitOfWork.Save();
        return unit.Id;
    }

    public void Update(int id, UpdateUnitDto dto)
    {
        var unit = unitRepository.Find(id)
                   ?? throw new UnitNotFoundException();

        var floor = floorRepository.Find(unit.FloorId);
        if (floorRepository.UnitsCount(unit.FloorId) >= floor.UnitCount)
            throw new FloorHasMaxUnitsCountException();

        var isDuplicate = unitRepository.IsDuplicate(dto.Name);
        if (isDuplicate)
            throw new UnitNameDuplicateException();


        unit.Name = dto.Name;
        unit.IsActive = dto.IsActive;
        unit.FloorId = dto.FloorId;
        unitRepository.Update(unit);
        unitOfWork.Save();
    }

    public void Delete(int id)
    {
        var unit = unitRepository.Find(id)
                   ?? throw new UnitNotFoundException();

        unitRepository.Remove(unit);
        unitOfWork.Save();
    }

    public void AddRange(int floorId, List<AddUnitDto> unitDTOs)
    {
        var units = unitDTOs.Select(d => new Unit
        {
            Name = d.Name,
            FloorId = floorId,
            IsActive = d.IsActive
        }).ToList();
        unitRepository.AddRange(units);
        unitOfWork.Save();
    }

    public void UpdateRange(int floorId,
        HashSet<UpdateUnitsOfFloorDto> updateDTOs)
    {
        var unitIds = updateDTOs.Select(u => u.Id).ToList();
        var units = unitRepository.FindByIds(floorId,unitIds);

        if (units.Count != updateDTOs.Count)
            throw new OneOrMoreUnitNotFoundException();
        
        units.ForEach(unit =>
        {
            var dto = updateDTOs.First(d=>d.Id==unit.Id);
            unit.IsActive = dto.IsActive;
            unit.Name = dto.Name;
        });
        
        unitRepository.UpdateRange(units);
        unitOfWork.Save();
    }
}