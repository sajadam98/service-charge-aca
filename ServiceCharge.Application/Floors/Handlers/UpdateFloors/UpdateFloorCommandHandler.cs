using ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts;
using ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts.DTOs;
using ServiceCharge.Services.Units.Contracts.Dtos;

namespace ServiceCharge.Application.Floors.Handlers.AddFloors;

public class UpdateFloorCommandHandler(
    FloorService floorService,
    UnitService unitService,
    UnitOfWork unitOfWork) : UpdateFloorHandler
{
    public void Handle(int blockId, int floorId, UpdateFloorWithUnitsCommand command)
    {
        unitOfWork.Begin();
        try
        {
            var updateFloorDto = new UpdateFloorDto
            {
                Name = command.Name,
                UnitCount = command.UnitCount
            };
            floorService.Update(floorId, blockId, updateFloorDto);
            
            unitService.AddRange(floorId, command.Units.Select(u => 
                new AddUnitDto
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