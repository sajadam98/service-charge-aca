namespace ServiceCharge.Application.Floors.Handlers.DeleteFloors.Contracts;

public interface DeleteFloorHandler
{
    void Handle(int floorId);
}