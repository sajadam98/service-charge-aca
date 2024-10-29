using ServiceCharge.Entities;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.Floors.Contracts.Dto;


namespace ServiceCharge.Persistence.Ef.Floors;


public class EFFloorQuery(EfDataContext context) : FloorQuery
{
    public List<GetAllFloorsBlockDto> GetAll(int blockId)
    {
        return context.Set<Floor>().Where(_ => _.BlockId == blockId).Select(_ => new GetAllFloorsBlockDto()
        {
        Name = _.Name,
        UnitCount = _.UnitCount
        }).ToList();
    }
}