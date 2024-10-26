namespace ServiceCharge.Services.Blocks.Contracts.Interfaces;

public interface BlockService
{
    void Add(AddBlockDto dto);
    void AddWithFloor(AddBlockWithFloorDto dto);
    void Update(int id, UpdateBlockDto dto);
    void Delete(int id);
}