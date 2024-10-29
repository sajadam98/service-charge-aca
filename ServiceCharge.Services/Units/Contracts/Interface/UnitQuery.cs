using ServiceCharge.Services.Units.Contracts.Dtos;

namespace ServiceCharge.Services.Units.Contracts.Interface;

public interface UnitQuery
{
    List<GetAllFloorsAndUnitsDto> GetAllFloorsAndUnits();
}