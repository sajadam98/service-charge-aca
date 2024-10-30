namespace ServiceCharge.Application.Floors.Handler.UpdateFloorWithUnits;

public class UpdateFloorWithUnitsCommandHandler(
    UnitOfWork unitOfWork,
    FloorService floorService,
    UnitService unitService) : UpdateFloorWithUnitsHandler
{
    public void Handle(int floorId, UpdateFloorWithUnitsCommand command)
    {
        unitOfWork.Begin();
        try
        {
            var floorDto = new UpdateFloorDto()
            {
                Id = floorId,
                Name = command.Name,
                UnitCount = command.UnitCount
            };
            floorService.Update(floorDto);
            unitService.UpdateRange(floorId, command.Units.Select(u =>
                new UpdateUnitDto()
                {
                    Id = u.Id,
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