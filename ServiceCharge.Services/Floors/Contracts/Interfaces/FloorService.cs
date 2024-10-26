namespace ServiceCharge.Services.Floors.Contracts.Interfaces;

public interface FloorService
{
    int Add(int id, AddFloorDto dto);
    void Update(int floorId, UpdateFloorDto dto);
    void Delete(int id);
}