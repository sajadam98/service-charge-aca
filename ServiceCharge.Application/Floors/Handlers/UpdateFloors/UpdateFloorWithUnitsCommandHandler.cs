using ServiceCharge.Application.Floors.Handlers.UpdateFloors.DTOs;
using ServiceCharge.Services.Units.Contracts.Dtos;

namespace ServiceCharge.Application.Floors.Handlers.UpdateFloors;

public class UpdateFloorWithUnitsCommandHandler(FloorService floorService,
    UnitService unitService,
    UnitOfWork unitOfWork):UpdateFloorWithUnitsHandler
{
    public void Handle(UpdateFloorWithUnitsCommand command, int floorId)
    {
        unitOfWork.Begin();

        try
        {
            floorService.Update(floorId, new UpdateFloorDto()
            {
                Name = command.Name,
                UnitCount = command.UnitCount,
            });

            for (int i = 0; i < command.Units.Count; i++)
            {
                unitService.Update(command.UnitsId[i], new UpdateUnitDto()
                {
                    Name = command.Units[i].Name,
                    IsActive = command.Units[i].IsActive,
                });
            }
            
            unitOfWork.Commit();
        }
        catch (Exception e)
        {
            unitOfWork.Rollback();
            throw;
        }
    }
}