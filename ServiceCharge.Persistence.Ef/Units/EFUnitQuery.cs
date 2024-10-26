namespace ServiceCharge.Persistence.Ef.Units;

public class EFUnitQuery(EfDataContext context) : UnitQuery
{
    public HashSet<GetAllUnitsDto> GetAll()
    {
        return context.Set<Unit>().Select(_ => new GetAllUnitsDto()
        {
            Id = _.Id,
            Name = _.Name,
            FloorId = _.FloorId,
            IsActive = _.IsActive,
        }).ToHashSet();
    }
    public HashSet<GetAllUnitsWithBlockNameAndFloorNameDto> GetAllWithBlockNameAndFloorName()
    {
        var query = (from unit in context.Set<Unit>()
        join floor in context.Set<Floor>() on unit.FloorId equals floor.Id
        select new GetAllUnitsWithBlockNameAndFloorNameDto
        {
            BlockName = floor.Block.Name,
            Name = unit.Name,
            FloorName = floor.Name,
            IsActive = unit.IsActive,
            FloorId = floor.Id,
            Id = unit.Id
        }).ToHashSet();
        return query;
    }
    public Unit GetById(int id)
    {
        return context.Set<Unit>().FirstOrDefault(_ => _.Id == id);
    }
}