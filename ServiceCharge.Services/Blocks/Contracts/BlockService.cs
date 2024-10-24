namespace ServiceCharge.Services.Blocks.Contracts;

public interface BlockService
{
    void Add(AddBlockDto dto);
    void AddWithFloor(AddBlockWithFloorDto dto);
    void Update(PutBlockDto dto);
}