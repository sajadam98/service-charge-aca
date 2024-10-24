using ServiceCharge.Services.Floors.Contracts.Dtos;

namespace ServiceCharge.Services.Floors.Contracts;

public interface FloorQuery
{
    HashSet<GetFloorDto> GetAll();
    HashSet<GetFloorWithUnitsDto> GetAllWithUnits();
}