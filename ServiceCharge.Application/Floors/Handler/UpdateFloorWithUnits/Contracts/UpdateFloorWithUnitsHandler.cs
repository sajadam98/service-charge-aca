namespace ServiceCharge.Application.Floors.Handler.UpdateFloorWithUnits.
    Contracts;

public interface UpdateFloorWithUnitsHandler
{
    void Handle(int floorId, UpdateFloorWithUnitsCommand command);
}
