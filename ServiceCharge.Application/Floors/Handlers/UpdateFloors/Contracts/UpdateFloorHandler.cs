using ServiceCharge.Application.Floors.Handlers.UpdateFloors.Contracts.DTOs;

namespace ServiceCharge.Application.Floors.Handlers.UpdateFloors.Contracts;

public interface UpdateFloorHandler
{
    void Handle(int floorId, UpdateFloorWithUnitsCommand command);
}