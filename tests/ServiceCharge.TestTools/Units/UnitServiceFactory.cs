using ServiceCharge.Persistence.Ef;
using ServiceCharge.Persistence.Ef.Floors;
using ServiceCharge.Persistence.Ef.UnitOfWorks;
using ServiceCharge.Persistence.Ef.Units;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.Units;
using ServiceCharge.Services.Units.Contracts;

namespace ServiceCharge.TestTools.Units;

public class UnitServiceFactory
{
    public static UnitService CreateUnitService(EfDataContext context,
        UnitRepository? repository = null)
    {
        repository ??= new EFUnitRepository(context);
        
        var floorRepository = new  EFFloorRepository(context);
        var unitOfWork = new EfUnitOfWork(context);
        
        return new UnitAppService(repository,
            unitOfWork,
            floorRepository);
    }
}