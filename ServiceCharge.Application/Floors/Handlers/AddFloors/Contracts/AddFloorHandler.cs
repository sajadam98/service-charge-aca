using ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts.DTOs;

namespace ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts;

public interface AddFloorHandler
{
    void Handle(int blockId, AddFloorWithUnitsCommand command);
}