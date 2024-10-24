using ServiceCharge.Application.CreateFloorWithUnits.Contracts;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.UnitOfWorks;
using ServiceCharge.Services.Units.Contracts;

namespace ServiceCharge.Application.CreateFloorWithUnits;

public class CreateFloorWithUnitsCommandHandler:CreateFloorWithUnitsHandler
{
    private readonly UnitOfWork _context;
    private readonly FloorService _floorService;
    private readonly UnitsService _unitsService;

    public CreateFloorWithUnitsCommandHandler(
        UnitOfWork context,
        FloorService floorService,
        UnitsService unitsService)
    {
        _unitsService = unitsService;
        _unitsService = unitsService;
        _floorService = floorService;
    }
}