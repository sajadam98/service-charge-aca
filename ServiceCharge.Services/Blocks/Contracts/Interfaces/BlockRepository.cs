using ServiceCharge.Services.Floors.Contracts.Dto;

namespace ServiceCharge.Services.Blocks.Contracts.Interfaces;

public interface BlockRepository
{
    void Add(Block block);
    bool IsDuplicate(string name);
    GetBlockFloorCapacityAndFloorCountDto? FindById(int blockId);
    Block Find(int id);
    void Update(Block block);
    void Remove(Block block);
}