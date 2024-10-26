namespace ServiceCharge.Services.Units.Contracts.Interfaces;

public interface UnitQuery
{
    HashSet<GetAllUnitsDto> GetAll();
    HashSet<GetAllUnitsWithBlockNameAndFloorNameDto> GetAllWithBlockNameAndFloorName();
    Unit GetById(int id);
}