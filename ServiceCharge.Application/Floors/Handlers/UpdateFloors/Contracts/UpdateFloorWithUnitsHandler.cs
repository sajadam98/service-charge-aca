using System.Reflection.Metadata;
using ServiceCharge.Application.Floors.Handlers.UpdateFloors.DTOs;

namespace ServiceCharge.Application.Floors.Handlers.UpdateFloors;

public interface UpdateFloorWithUnitsHandler
{
    void Handle(UpdateFloorWithUnitsCommand command, int floorId);
}