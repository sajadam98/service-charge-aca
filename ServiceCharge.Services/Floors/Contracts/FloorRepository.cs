namespace ServiceCharge.Services.Floors.Contracts;

public interface FloorRepository
{
    void Add(Floor floor);
    bool IsNameDuplicateInBlock(int blockId, string name);
    
    int RegisterableFloorCount(int blockId);
    Floor? FindById(int id);
    bool IsNameDuplicate(string? name);
}