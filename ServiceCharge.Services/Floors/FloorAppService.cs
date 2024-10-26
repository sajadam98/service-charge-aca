namespace ServiceCharge.Services.Floors;

public class FloorAppService(
    FloorRepository floorRepository,
    BlockRepository blockRepository,
    UnitOfWork unitOfWork) : FloorService
{
    public int Add(int id, AddFloorDto dto)
    {
        var isDuplicate = floorRepository.IsDuplicate(dto.Name);
        if (isDuplicate)
            throw new FloorNameDuplicateException();
        
        var block = blockRepository.FindById(id)
                    ?? throw new BlockNotFoundException();
        
        if (block.FloorCapacity <= block.FloorCount)
            throw new BlockHasMaxFloorsCountException();

        var floor = new Floor()
        {
            Name = dto.Name,
            UnitCount = dto.UnitCount,
            BlockId = id,
        };
        floorRepository.Add(floor);
        unitOfWork.Save();
        return floor.Id;
    }
    public void Update(int floorId, UpdateFloorDto dto)
    {
        var isDuplicate = floorRepository.IsDuplicate(dto.Name);
        if (isDuplicate)
            throw new FloorNameDuplicateException();
        
        var floor = floorRepository.Find(floorId)
                    ?? throw new FloorNotFoundException();

        if (dto.UnitCount < floorRepository.UnitsCount(floorId))
            throw new FloorHasMaxUnitsCountException();

        floor.Name = dto.Name;
        floor.UnitCount = dto.UnitCount;
        floorRepository.Update(floor);
        unitOfWork.Save();
    }

    public void Delete(int id)
    {
        var floor = floorRepository.Find(id)
                    ?? throw new FloorNotFoundException();
        
        var block = blockRepository.Find(floor.BlockId);
        
        if (floorRepository.UnitsCount(id) > 0)
            throw new FloorHasUnitsException();
        
        floorRepository.Remove(floor);
        unitOfWork.Save();
    }
}