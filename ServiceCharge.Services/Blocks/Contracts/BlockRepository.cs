using ServiceCharge.Entities;

namespace ServiceCharge.Services.Blocks.Contracts;

public interface BlockRepository
{
    void Add(Block block);
    bool IsDuplicate(string name);
    bool IsExistById(int blockId);
    Block? FindById(int id);
    GetBlockInformationDto GetBlockInformationById (int id);
}