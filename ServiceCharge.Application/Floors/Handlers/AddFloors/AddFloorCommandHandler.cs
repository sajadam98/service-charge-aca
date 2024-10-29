using ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts;
using ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts.DTOs;
using ServiceCharge.Services.Units.Contracts.Dtos;

namespace ServiceCharge.Application.Floors.Handlers.AddFloors;

public class AddFloorCommandHandler(
    FloorService floorService,
    UnitService unitService,
    UnitOfWork unitOfWork) : AddFloorHandler
{
    public void Handle(int blockId, AddFloorWithUnitsCommand command)
    {
        unitOfWork.Begin();
        try
        {
            var addFloorDto = new AddFloorDto
            {
                Name = command.Name,
                UnitCount = command.UnitCount
            };
            var floorId = floorService.Add(blockId, addFloorDto);
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