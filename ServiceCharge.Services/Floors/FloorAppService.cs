using System.Data;
using System.Security.AccessControl;
using ServiceCharge.Entities;
using ServiceCharge.Services.Blocks.Exceptions;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.Floors.Contracts.Dto;
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

    public void Update(int floorId, UpdateFloorDto dto)
    {
        var floor = repository.FindById(floorId);
        if (floor == null)
        {
            throw new FloorNotFoundException();
        }

        if (IsDuplicateName(floorId,dto.Name, floor.BlockId))
        {
            throw new DuplicateFloorNameException();
        }
        floor.Name = dto.Name;
        floor.UnitCount = dto.UnitCount;
        
        unitOfWork.Save();
    }

    public void Delete(int floorId)
    {
        var floor = repository.FindById(floorId);
        if (floor == null)
        {
            throw new FloorNotFoundException();
        }
        repository.Delete(floor);
        
        unitOfWork.Save();
    }

    private bool IsDuplicateName(int floorId, string dtoName, int blockId)
    {
        if (repository.IsFloorExistWithSameNameButNotPreviousName(dtoName, floorId, blockId))
            return true;
        return false;
    }
}