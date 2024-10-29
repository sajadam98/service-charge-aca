using ServiceCharge.Entities;
using ServiceCharge.Services.Blocks.Exceptions;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.Floors.Exceptions;
using ServiceCharge.Services.UnitOfWorks;
using AddFloorDto =
    ServiceCharge.Services.Floors.Contracts.Dto.AddFloorDto;

namespace ServiceCharge.Services.Floors;

public class FloorAppService(
    FloorRepository repository,
    BlockRepository blockRepository,
    UnitOfWork unitOfWork)
    : FloorService
{
    public int Add(int blockId, AddFloorDto dto)
    {
        var block = blockRepository.FindById(blockId);
        if (block is null)
        {
            throw new BlockNotFoundException();
        }

        if (block.FloorCapacity <= block.FloorCount)
        {
            throw new BlockFloorsCapacityFulledException();
        }

        var isFloorExistWithSameName =
            repository.IsFloorExistWithSameName(dto.Name, blockId);
        if (isFloorExistWithSameName)
        {
            throw new DuplicateFloorNameException();
        }

        var floor = new Floor
        {
            Name = dto.Name,
            UnitCount = dto.UnitCount,
            BlockId = blockId
        };
        repository.Add(floor);
        unitOfWork.Save();
        return floor.Id;
    }

    public void Update(int blockId, string name, string updatename)
    {
        var block = blockRepository.FindById(blockId);
        if (block is null)
        {
            throw new BlockNotFoundException();
        }
        var exist=repository.IsFloorExistWithSameName(name, blockId);
        if (!exist)
        {
            throw new FloorIsentExistException();
        }

        if (name==updatename)
        {
            throw new DuplicateFloorNameException();
        }
        var floor=repository.FindByName(name);
        
        floor.Name=updatename;
        
        unitOfWork.Save();
        
    }

    public void Delete(int blockId, string floorName)
    {
        var block = blockRepository.FindById(blockId);
        if (block is null)
        {
            throw new BlockNotFoundException();
        }
        var exist=repository.IsFloorExistWithSameName(floorName, blockId);
        if (!exist)
        {
            throw new FloorIsentExistException();
        }
        repository.Delete(floorName);
        unitOfWork.Save();
        
    }
}

public class FloorIsentExistException : Exception
{
}