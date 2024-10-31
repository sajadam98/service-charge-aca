using Moq;
using ServiceCharge.Persistence.Ef;
using ServiceCharge.Persistence.Ef.Blocks;
using ServiceCharge.Persistence.Ef.UnitOfWorks;
using ServiceCharge.Services;
using ServiceCharge.Services.Blocks;
using ServiceCharge.Services.Blocks.Contracts.Interfaces;

namespace ServiceCharge.TestTools.Blocks;

public static class BlockServiceFactory
{
    public static BlockService CreateService(EfDataContext context,
        DateTime? fakeDateTime = null)
    {
        var blockRepository = new EFBlockRepository(context);
        var unitOfWork = new EfUnitOfWork(context);
        var dateTimeService = new Mock<DateTimeService>();
        dateTimeService.Setup(s => s.NowUtc)
            .Returns(fakeDateTime ?? new DateTime(2022, 01, 01));
        return new BlockAppService(
            blockRepository,
            unitOfWork,
            dateTimeService.Object);
    }
}