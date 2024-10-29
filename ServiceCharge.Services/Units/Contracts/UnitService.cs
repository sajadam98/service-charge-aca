namespace ServiceCharge.Services.Units.Contracts;

public interface UnitService
{
    int Add(int floorId, AddUnitDto dto);
    void Update(int unitId, UpdateUnitDto dto);
    void Delete(int unit2Id);
}