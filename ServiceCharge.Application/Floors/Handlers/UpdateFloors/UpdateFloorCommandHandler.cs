using ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts;
using ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts.DTOs;
using ServiceCharge.Services.Units.Contracts.Dtos;

namespace ServiceCharge.Application.Floors.Handlers.UpdateFloors;

public class UpdateFloorCommandHandler(
    FloorService floorService,
    UnitService unitService,
    UnitOfWork unitOfWork) : AddFloorHandler
{
    public int Handle(int floorId, AddFloorWithUnitsCommand command)
    {
        unitOfWork.Begin();
        try
        {
            var updateFloorDto = new UpdateFloorDto
            {
                Name = command.Name,
                UnitCount = command.UnitCount
            };
            floorService.Update(floorId, updateFloorDto);
            unitService.AddRange(floorId, command.Units.Select(u =>
                new AddUnitDto
                {
                    Name = u.Name,
                    IsActive = u.IsActive
                }).ToList());
            unitOfWork.Commit();
            return floorId;
        }
        catch (Exception e)
        {
            unitOfWork.Rollback();
            throw;
        }
    }
}