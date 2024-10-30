namespace ServiceCharge.Services.Unit;

public class UnitAppService(
    UnitOfWork unitOfWork,
    UnitRepository repository,
    FloorRepository floorRepository) : UnitService
{
    public int Add(int floorId, AddUnitDto dto)
    {
        var floor = floorRepository.GetFloorInfoById(floorId);
        if (floor is null)
            throw new FloorNotFoundException();
        if (floor.UnitCount >= floor.UnitCapacity)
            throw new FloorReachedMaxUnitException();
        
        var unit = new Entities.Unit()
        {
            Name = dto.Name,
            IsActive = dto.IsActive,
            FloorId = floorId
        };
        repository.Add(unit);
        unitOfWork.Save();
        
        return unit.Id;
    }

    public void AddRange(int floorId, List<AddUnitDto> unitDtos)
    {
        throw new NotImplementedException();
    }
}