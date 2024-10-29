using ServiceCharge.Entities;
using ServiceCharge.Services.Blocks.Exceptions;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.Floors.Exceptions;
using ServiceCharge.Services.UnitOfWorks;
using AddFloorDto =
    ServiceCharge.Services.Floors.Contracts.Dto.AddFloorDto;

namespace ServiceCharge.Services.Floors;

public class FloorAppService(
    FloorRepository floorRepository,
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
            floorRepository.IsFloorExistWithSameName(dto.Name, blockId);
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
        floorRepository.Add(floor);
        unitOfWork.Save();
        return floor.Id;
    }

    public void Update(int floorId, UpdateFloorDto dto)
    {
        var floorDto = floorRepository.FindByIdWithExistingUnitsCount(floorId);

        if (floorDto is null)
            throw new FloorNotFoundException();

        if (floorDto.Floor!.Name != dto.Name)
        {
            var isFloorExistWithSameName =
                floorRepository.IsFloorExistWithSameName(dto.Name,
                    floorDto.Floor!.BlockId);
            if (isFloorExistWithSameName)
            {
                throw new DuplicateFloorNameException();
            }
        }

        if (floorDto.ExistingUnitsCount > dto.UnitCount)
            throw new FloorUnitsCountException();

        floorDto.Floor!.UnitCount = dto.UnitCount;
        floorDto.Floor!.Name = dto.Name;

        floorRepository.Update(floorDto.Floor!);
        unitOfWork.Save();
    }

    public void Delete(int floorId)
    {
        var floorDto = floorRepository.FindByIdWithExistingUnitsCount(floorId);

        if (floorDto is null)
            throw new FloorNotFoundException();

        if (floorDto.ExistingUnitsCount > 0)
            throw new FloorUnitsCountException();

        floorRepository.Delete(floorDto.Floor);
        unitOfWork.Save();
    }
}