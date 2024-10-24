using System.Data;
using ServiceCharge.Entities;
using ServiceCharge.Services.Blocks.Exceptions;
using ServiceCharge.Services.UnitOfWorks;

namespace ServiceCharge.Services.Blocks;

public class BlockAppService(
    BlockRepository blockRepository,
    UnitOfWork unitOfWork,
    DateTimeService dateTimeService) : BlockService
{
    public void Add(AddBlockDto dto)
    {
        var isDuplicate = blockRepository.IsDuplicate(dto.Name);
        if (isDuplicate)
            throw new BlockNameDuplicateException();

        var block = new Block()
        {
            Name = dto.Name,
            FloorCount = dto.FloorCount,
            CreationDate = dateTimeService.NowUtc
        };

        blockRepository.Add(block);
        unitOfWork.Save();
    }

    public void AddWithFloor(AddBlockWithFloorDto dto)
    {
        var isDuplicate = blockRepository.IsDuplicate(dto.Name);
        if (isDuplicate)
            throw new BlockNameDuplicateException();

        var block = new Block()
        {
            Name = dto.Name,
            FloorCount = dto.Floors.Count,
            CreationDate = dateTimeService.NowUtc
        };

        block.Floors = dto.Floors.Select(_ => new Floor()
        {
            Name = _.Name,
            UnitCount = _.UnitCount
        }).ToHashSet();

        blockRepository.Add(block);
        unitOfWork.Save();
    }

    public void Update(PutBlockDto dto)
    {
        Block? block = blockRepository.FindById(dto.Id);
        
        if (block is null)
        throw new BlockNotFoundException();
        
        if (block.Name != dto.Name)
            if (blockRepository.IsNameDuplicate(dto.Name))
                throw new BlockNameDuplicateException();
        
        
        
        block.Name = dto.Name is null ? block.Name : dto.Name;
        block.CreationDate = dto.CreationDate is null
            ? block.CreationDate
            : (DateTime)dto.CreationDate;
        block.FloorCount =
            dto.FloorCount is null ? block.FloorCount : (int)dto.FloorCount;

        unitOfWork.Save();
    }
}