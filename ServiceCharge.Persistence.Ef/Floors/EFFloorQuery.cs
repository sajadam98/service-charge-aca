namespace ServiceCharge.Persistence.Ef.Floors;

public class EFFloorQuery(EfDataContext context) : FloorQuery
{
    public HashSet<GetAllFloorsDto> GetAll()
    {
        return context.Set<Floor>().Select(_ => new GetAllFloorsDto
        {
            Id = _.Id,
            Name = _.Name,
            UnitCount = _.UnitCount,
            BlockId = _.BlockId,
        }).ToHashSet();
    }
    public HashSet<GetAllFloorsWithUnitsDto> GetAllFloorsWithUnits()
    {
        return context.Set<Floor>().Select(_ => new GetAllFloorsWithUnitsDto
            {
                Id = _.Id,
                Name = _.Name,
                UnitCount = _.UnitCount,
                BlockId = _.BlockId,
                Units = context.Set<Unit>().Where(u => u.FloorId == _.Id).Select(
                    _ => new GetAllUnitsDto()
                    {
                        Id = _.Id,
                        Name = _.Name,
                        IsActive = _.IsActive,
                        FloorId = _.FloorId,
                    }).ToHashSet()
            }).ToHashSet();
    }

    public Floor GetById(int id)
    {
        return context.Set<Floor>().FirstOrDefault(_ => _.Id == id);
    }
}