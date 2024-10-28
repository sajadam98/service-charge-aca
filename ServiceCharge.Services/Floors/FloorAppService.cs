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
        var isBlockExist = blockRepository.IsExistById(blockId);
        if (!isBlockExist)
        {
            throw new BlockNotFoundException();
        }

        if(!ValidateAddFloor(blockId))
        {
            throw new MaxCountFloorException();
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
    
    private bool ValidateAddFloor(int blockId)
    {
        var block = blockRepository.FindWithFloors(blockId);

        if (!(block!.FloorCount > block.Floors.Count))
            return false;
        return true;
    }
}