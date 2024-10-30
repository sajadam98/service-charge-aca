namespace ServiceCharge.Application.Floors.Handler.AddFloorWithUnits.Contracts;

public interface AddFloorWitUnitsHandler
{
    void Handle(int blockId, AddFloorWithUnitsCommand command);
}