namespace ServiceCharge.Services.Units.Contracts.Interfaces;

public interface UnitService
{
    int Add(int id, AddUnitDto dto);
    void Update(int id, UpdateUnitDto dto);
    void Delete(int id);
    void AddRange(List<Unit> units);
}