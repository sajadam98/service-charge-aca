using ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts.DTOs;

namespace ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts;

public interface UpdateFloorHandler
{
    void Handle(int floorId,int blockId, UpdateFloorWithUnitsCommand command);
}