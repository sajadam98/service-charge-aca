using ServiceCharge.Entities;
using ServiceCharge.Services.Units.Contracts.Dtos;
using ServiceCharge.Services.Units.Contracts.Interface;

namespace ServiceCharge.Persistence.Ef.Units;

public class EFUnitQuery(EfDataContext context):UnitQuery
{
    public List<GetAllFloorsAndUnitsDto> GetAllFloorsAndUnits()
    {
        return (
            from unit in context.Set<Unit>()
            join floor in context.Set<Floor>() on unit.FloorId equals floor.Id into floorUnits
            from floor in floorUnits.DefaultIfEmpty()
            select new GetAllFloorsAndUnitsDto
            {
                FloorName = floor.Name, 
                BlockName = floor.Block.Name, 
                UnitName = unit.Name,
                IsActive = unit.IsActive
            }).ToList();
    }


}