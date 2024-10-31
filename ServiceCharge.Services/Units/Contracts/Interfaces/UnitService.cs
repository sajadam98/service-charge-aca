namespace ServiceCharge.Services.Units.Contracts.Interfaces;

public interface UnitService
{
    int Add(int id, AddUnitDto dto);
    void Update(int id, UpdateUnitDto dto);
    void Delete(int id);
    void AddRange(int floorId, List<AddUnitDto> unitDTOs);
    void UpdateRange(int floorId, HashSet<UpdateUnitsOfFloorDto> updateDTOs);
}

public class UpdateUnitsOfFloorDto()
{
    public required string Name { get; set; }
    public int UnitCount { get; set; }
    public int Id { get; set; }
    public bool IsActive { get; set; }
}