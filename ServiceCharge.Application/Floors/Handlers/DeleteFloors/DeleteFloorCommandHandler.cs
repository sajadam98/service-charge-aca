using ServiceCharge.Application.Floors.Handlers.DeleteFloors.Contracts;

namespace ServiceCharge.Application.Floors.Handlers.DeleteFloors;

public class DeleteFloorCommandHandler
(
    FloorService floorService,
    UnitService unitService,
    UnitOfWork unitOfWork) : DeleteFloorHandler
{
    public void Handle(int floorId)
    {
        unitOfWork.Begin();
        try
        {
            unitService.DeleteByFloorId(floorId);
            
            floorService.Delete(floorId);

            unitOfWork.Commit();
        }
        catch (Exception)
        {
            unitOfWork.Rollback();
            throw;
        }
    }
}