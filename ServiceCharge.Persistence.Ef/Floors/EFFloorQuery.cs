using ServiceCharge.Entities;
using ServiceCharge.Services.Floors.Contracts;

namespace ServiceCharge.Persistence.Ef.Floors;

public class EFFloorQuery(EfDataContext context) : FloorQuery
{
    public List<GetAllFloorsDto> GetAll()
    {
        return context.Set<Floor>().Select((f => new GetAllFloorsDto
        {
            Name = f.Name,
            UnitCount = f.UnitCount,
            BlockId = f.BlockId,
            Id = f.Id,
        })).ToList();
    }
}