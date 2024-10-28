using ServiceCharge.Entities;
using ServiceCharge.Services.Floors.Contracts.Dto;

namespace ServiceCharge.Services.Blocks.Contracts;

public interface BlockRepository
{
    void Add(Block block);
    bool IsDuplicate(string name);
    GetBlockFloorCapacityAndFloorCountDto? FindById(int blockId);
}