using ServiceCharge.Entities;

namespace ServiceCharge.Services.Floors.Contracts;

public interface FloorRepository
{
    void Add(Floor floor);
    bool IsFloorExistWithSameName(string name, int blockId);
    FindByIdWithUnitsCountDto? FindByIdWithExistingUnitsCount(int floorId);
    void Update(Floor floor);
    void Delete(Floor floor);
}

public class FindByIdWithUnitsCountDto
{
    public Floor? Floor { get; set; }
    public int ExistingUnitsCount { get; set; }
}