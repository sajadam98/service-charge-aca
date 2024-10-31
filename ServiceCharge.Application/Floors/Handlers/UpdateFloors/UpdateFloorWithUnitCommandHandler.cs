using ServiceCharge.Application.Floors.Handlers.UpdateFloors.Contracts;
using ServiceCharge.Application.Floors.Handlers.UpdateFloors.Contracts.DTOs;

namespace ServiceCharge.Application.Floors.Handlers.UpdateFloors;

public class UpdateFloorWithUnitCommandHandler(
    FloorService floorService,
    UnitService unitService,
    UnitOfWork unitOfWork) : UpdateFloorHandler
{
    public void Handle(int floorId, UpdateFloorWithUnitsCommand command)
    {
        unitOfWork.Begin();
        try
        {
            var floorDto = new UpdateFloorDto
            {
                Name = command.Name,
                UnitCount = command.UnitCount
            };
            floorService.Update(floorId, floorDto);

            var unitDtos = command.Units
                .Select(d => new UpdateUnitsOfFloorDto
                {
                    Name = d.Name,
                    IsActive = d.IsActive,
                    Id = d.Id,
                }).ToHashSet();
            unitService.UpdateRange(floorId, unitDtos);
            unitOfWork.Commit();
        }

        catch (Exception e)
        {
            unitOfWork.Rollback();
            throw;
        }
    }
}