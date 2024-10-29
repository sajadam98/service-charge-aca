using System.Data;
using ServiceCharge.Entities;
using ServiceCharge.Services.Blocks.Exceptions;
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
        var block = blockRepository.GetBlockInformationById(blockId);
        if (block is null)
            throw new BlockNotFoundException();

        if (block.FloorCapacity <= block.FloorCount)
            throw new BlockReachedMaxFloorException();

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

    public void Update(PutFloorDto dto)
    {
        var floor = repository.FindById(dto.Id);
        if (floor is null)
            throw new FloorNotFoundException();
        if (floor.Name != dto.Name)
        {
            if (repository.IsFloorExistWithSameName(dto.Name,floor.BlockId))
                throw new DuplicateFloorNameException();
        }

        var floorInfo = repository.GetFloorInfoById(dto.Id);
        if (floorInfo.UnitCount > dto.UnitCount)
            throw new UnitCountisLessThanExistenceUnitsException();

        floor.Name = dto.Name;
        floor.UnitCount = dto.UnitCount;

        unitOfWork.Save();
    }

    public void Delete(int id)
    {
        var floor = repository.FindById(id);
        if (floor is null)
            throw new FloorNotFoundException();
        repository.Delete(floor);
        unitOfWork.Save();
    }
}