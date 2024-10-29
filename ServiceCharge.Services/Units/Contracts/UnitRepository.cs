using ServiceCharge.Entities;

namespace ServiceCharge.Services.Units.Contracts
{
    public interface UnitRepository
    {
        void Add(Unit unit);
        Unit FindById(int unitId);
        void Update(Unit unit);
        void Delete(Unit unit);
    }
}