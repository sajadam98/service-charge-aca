namespace ServiceCharge.Services.Floors.Contracts.Interfaces;

public interface FloorRepository
{
    void Add(Floor floor);
    bool IsDuplicate(string name);
    Floor Find(int id);
    int UnitsCount(int id);
    void Update(Floor floor);
    void Remove(Floor floor);
}