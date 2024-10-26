namespace ServiceCharge.Services.Floors.Contracts.Interfaces;

public interface FloorQuery
{
    HashSet<GetAllFloorsDto> GetAll();
    HashSet<GetAllFloorsWithUnitsDto> GetAllFloorsWithUnits();
    Floor GetById(int id);
}