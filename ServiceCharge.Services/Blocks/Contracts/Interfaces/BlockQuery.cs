namespace ServiceCharge.Services.Blocks.Contracts.Interfaces;

public interface BlockQuery
{
    HashSet<GetAllBlocksDto> GetAll();

    GetByIdAndFloorsInfo GetByIdAndFloorsInfo(int id);

    HashSet<GetAllBlocksWithFloorsDto> GetAllWithFloorsInfo();
    Block GetById(int id);
}