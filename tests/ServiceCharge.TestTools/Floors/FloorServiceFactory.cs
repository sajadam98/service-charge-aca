using ServiceCharge.Persistence.Ef;
using ServiceCharge.Persistence.Ef.Blocks;
using ServiceCharge.Persistence.Ef.Floors;
using ServiceCharge.Persistence.Ef.UnitOfWorks;
using ServiceCharge.Services.Blocks.Contracts;
using ServiceCharge.Services.Floors;
using ServiceCharge.Services.Floors.Contracts;

namespace ServiceCharge.TestTools.Floors;

public static class FloorServiceFactory
{
    public static FloorService CreateService(
        EfDataContext context,
        FloorRepository? repository = null,
        BlockRepository? blockRepository = null)
    {
        repository ??= new EFFloorRepository(context);
        blockRepository ??= new EFBlockRepository(context);
        var unitOfWork = new EfUnitOfWork(context);
        return new FloorAppService(repository, blockRepository, unitOfWork);
    }
}