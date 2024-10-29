using ServiceCharge.Persistence.Ef;
using ServiceCharge.Persistence.Ef.Floors;
using ServiceCharge.Persistence.Ef.UnitOfWorks;
using ServiceCharge.Persistence.Ef.Units;
using ServiceCharge.Services.Units;
using ServiceCharge.Services.Units.Contracts.Interfaces;

namespace ServiceCharge.TestTools.Units;

public static class UnitServiceFactory
{
    public static UnitService CreateService(EfDataContext context)
    {
        var unitRepository = new EFUnitRepository(context);
        var unitOfWork = new EfUnitOfWork(context);
        var floorRepository = new EFFloorRepository(context);
        return new UnitAppService(
            unitRepository, 
            unitOfWork,
            floorRepository);
    }
}