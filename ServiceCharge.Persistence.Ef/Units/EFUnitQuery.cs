using ServiceCharge.Entities;
using ServiceCharge.Services.Units.Contracts;

namespace ServiceCharge.Persistence.Ef.Units;

public class EFUnitQuery(EfDataContext context) : UnitQuery
{
    public List<GetAllUnitsDto> GetAll()
    {
        return context.Set<Unit>().Select(u => new GetAllUnitsDto
        {
            Id = u.Id,
            Name = u.Name,
            IsActive = u.IsActive,
            FloorId = u.FloorId,
        }).ToList();
    }
}