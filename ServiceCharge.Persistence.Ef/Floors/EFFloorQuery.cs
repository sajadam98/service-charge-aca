using ServiceCharge.Entities;
using ServiceCharge.Services.Floors;
using ServiceCharge.Services.Floors.Contracts;

namespace ServiceCharge.Persistence.Ef.Floors;

public class EFFloorQuery(EfDataContext context):FloorQuery
{
    public HashSet<GetFloorDto> GetAll()
    {
        return context.Set<Floor>().Select(_ => new GetFloorDto()
        {
            Id = _.Id,
            Name = _.Name,
            UnitCount =_.UnitCount,
            BlockId = _.BlockId
        }).ToHashSet();
    }
}