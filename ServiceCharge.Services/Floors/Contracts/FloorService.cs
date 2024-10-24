namespace ServiceCharge.Services.Floors.Contracts;

public interface FloorService
{
    int Create(int blockId, AddFloorDto dto);
    void Update(PutFloorDto dto);
}