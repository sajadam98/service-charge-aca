namespace ServiceCharge.Services.Units.Contracts;

public interface UnitQuery
{
    HashSet<GetAllUnitsWithFloorAndBlockNameDto> GetAllUnitsWithFloorAndBlockName();
}