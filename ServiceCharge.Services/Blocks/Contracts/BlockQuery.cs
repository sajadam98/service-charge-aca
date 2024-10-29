namespace ServiceCharge.Services.Blocks.Contracts;

public interface BlockQuery
{
    HashSet<GetBlockDto> GetAll();
     HashSet<GetBlockWithFloorsDto> GetAllWithFloors();
     HashSet<GetAllWithAddedFloorCountDto> GetAllWithAddedFloorCount();
}