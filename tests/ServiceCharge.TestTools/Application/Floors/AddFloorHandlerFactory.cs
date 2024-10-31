using ServiceCharge.Application.Floors.Handlers.AddFloors;
using ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts;
using ServiceCharge.Persistence.Ef;
using ServiceCharge.Persistence.Ef.UnitOfWorks;
using ServiceCharge.Services.Floors.Contracts.Interfaces;
using ServiceCharge.Services.UnitOfWorks;
using ServiceCharge.Services.Units.Contracts.Interfaces;
using ServiceCharge.TestTools.Floors;
using ServiceCharge.TestTools.Units;

namespace ServiceCharge.TestTools.Application.Floors;

public static class AddFloorHandlerFactory
{
    public static AddFloorHandler CreateHandler(
        EfDataContext context,
        FloorService? floorService = null,
        UnitService? unitService = null,
        UnitOfWork? unitOfWork = null)
    {
        floorService ??= FloorServiceFactory.CreateService(context);
        unitService ??= UnitServiceFactory.CreateService(context);
        unitOfWork ??= new EfUnitOfWork(context);
        return new AddFloorCommandHandler(
            floorService,
            unitService,
            unitOfWork);
    }
}