using ServiceCharge.Services.Floors.Contracts.Dto;
using ServiceCharge.Services.Units.Contracts.Dtos;

namespace ServiceCharge.Services.Units.Contracts
{
    public interface UnitService
    {
        int Add(int floorId, AddUnitDto dto);
        void Update(int unitId, UpdateUnitDto dto);
        void Delete(int unitId);
    }
}