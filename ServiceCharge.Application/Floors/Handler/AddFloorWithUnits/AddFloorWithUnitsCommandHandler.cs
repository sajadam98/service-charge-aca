using ServiceCharge.Application.Floors.Handler.AddFloorWithUnits.Contracts;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.Unit;
using ServiceCharge.Services.UnitOfWorks;

namespace ServiceCharge.Application.Floors.Handler.AddFloorWithUnits;

public class AddFloorWithUnitsCommandHandler(
    UnitOfWork unitOfWork,
    FloorService floorService,
    UnitService unitService):AddFloorWitUnitsHandler
{
    
}