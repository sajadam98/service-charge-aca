namespace ServiceCharge.Services.Floors;

public class FloorAppService : FloorService
{
    private readonly UnitOfWork _context;
    private readonly FloorRepository _repository;
    private readonly BlockRepository _BlockRepository;

    public FloorAppService(
        UnitOfWork context,
        FloorRepository repository,
        BlockRepository blockRepository)
    {
        _context = context;
        _repository = repository;
        _BlockRepository = blockRepository;
    }

    public int Create(int blockId, AddFloorDto dto)
    {
        if (!_BlockRepository.IsExistById(blockId))
            throw new BlockNotFoundException();
        if (_repository.RegisterableFloorCount(blockId) <= 0)
            throw new ExceedsFloorLimitException();
        if (_repository.IsNameDuplicateInBlock(blockId, dto.Name))
            throw new FloorNameDuplicateInBlockException();

        var floor = new Floor()
        {
            BlockId = blockId,
            Name = dto.Name,
            UnitCount = dto.UnitCount
        };

        _repository.Add(floor);
        _context.Save();
        return floor.Id;
    }

    public void Update(PutFloorDto dto)
    {
        
        Floor? floor = _repository.FindById(dto.Id);
        if (floor is null)
            throw new FloorNotFoundException();
        if(floor.Name!=dto.Name)
            if (_repository.IsNameDuplicate(dto.Name))
                throw new FloorNameDuplicateInBlockException();
        floor.Name = string.IsNullOrEmpty(dto.Name) ? floor.Name : dto.Name;
        _context.Save();
    }
}