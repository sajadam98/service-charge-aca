using ServiceCharge.Persistence.Ef;
using ServiceCharge.Persistence.Ef.Floors;
using ServiceCharge.Persistence.Ef.UnitOfWorks;
using ServiceCharge.Persistence.Ef.Units;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.Unit;

namespace ServiceCharge.TestTools.Units;

public static class UnitServiceFactory
{
    public static UnitService CreateService(
        EfDataContext context,
        UnitRepository? repository = null,
        FloorRepository? floorRepository = null)
    {
        repository ??= new EfUnitRepository(context);
        floorRepository ??= new EFFloorRepository(context);
        var unitOfWork = new EfUnitOfWork(context);
        return new UnitAppService(unitOfWork, repository, floorRepository);
    }
}