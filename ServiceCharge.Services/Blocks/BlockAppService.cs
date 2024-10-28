﻿using ServiceCharge.Entities;
using ServiceCharge.Services.Blocks.Contracts.DTOs;
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
    
}