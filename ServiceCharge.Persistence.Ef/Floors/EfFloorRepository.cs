using ServiceCharge.Services.Floors.Contracts;

namespace ServiceCharge.Persistence.Ef.Floors;

public class EfFloorRepository : FloorRepository
{
    private readonly EfDataContext _context;

    public EfFloorRepository(EfDataContext context)
    {
        _context = context;
    }

    public void Add(Floor floor)
    {
        _context.Set<Floor>().Add(floor);
    }

    public bool IsNameDuplicateInBlock(int blockId, string name)
    {
        return _context.Set<Floor>()
            .Where(_ => _.BlockId == blockId)
            .Any(_ => _.Name == name);
    }

    public int RegisterableFloorCount(int blockId)
    {
        var block = _context.Set<Block>().Single(_ => _.Id == blockId);
        return block.FloorCount - block.Floors.Count;
    }

    public Floor? FindById(int id)
    {
        return _context.Set<Floor>().SingleOrDefault(_ => _.Id == id);
    }

    public bool IsNameDuplicate(string? name)
    {
        return _context.Set<Floor>().Any(_ => _.Name == name);
    }
}