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

    public void Update(int id, UpdateBlockDto dto)
    {
        var isDuplicate = blockRepository.IsDuplicate(dto.Name);
        if (isDuplicate) throw new BlockNameDuplicateException();
        
        var block = blockRepository.Find(id) 
                    ?? throw new BlockNotFoundException();
        block.Name = dto.Name;
        block.FloorCount = dto.FloorCount;
        blockRepository.Update(block);
        unitOfWork.Save();
    }

    public void Delete(int id)
    {
        var block = blockRepository.Find(id) 
                    ?? throw new BlockNotFoundException();
        blockRepository.Remove(block);
        unitOfWork.Save();
    }
}