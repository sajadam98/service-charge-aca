using ServiceCharge.Application.Floors.Handler.AddFloorWithUnits.Contracts;
using ServiceCharge.Services.Floors.Contracts.Dto;

namespace ServiceCharge.Application.Floors.Handler.AddFloorWithUnits;

public class AddFloorWithUnitsCommandHandler(
    UnitOfWork unitOfWork,
    FloorService floorService,
    UnitService unitService):AddFloorWitUnitsHandler
{
    public void Handle(int blockId, AddFloorWithUnitsCommand command)
    {
        unitOfWork.Begin();
        try
        {
            var floorDto = new AddFloorDto()
            {
                Name=command.Name,
                UnitCount = command.UnitCount
            };
            var floorId=floorService.Add(blockId, floorDto);
            unitService.AddRange(floorId,command.Units.Select(u=>new AddUnitDto()
            {
                Name = u.Name,
                IsActive = u.IsActive
            }).ToList());
            unitOfWork.Commit();
        }
        catch (Exception e)
        {
            unitOfWork.Rollback();
            throw;
        }
    }
}