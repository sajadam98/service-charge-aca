using ServiceCharge.Entities;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.UnitOfWorks;
using ServiceCharge.Services.Units.Contracts;
using ServiceCharge.Services.Units.Contracts.Dtos;

namespace ServiceCharge.Services.Units
{
    public class UnitAppService(
        FloorRepository floorRepository,
        UnitRepository unitRepository,
        UnitOfWork unitOfWork) : UnitService
    {
        public int Add(int floorId, AddUnitDto dto)
        {
            var unit = new Unit()
            {
                Name = dto.Name,
                IsActive = dto.IsActive,
                FloorId = floorId
            };
            unitRepository.Add(unit);
            unitOfWork.Save();
            return unit.Id;
        }

        public void Delete(int unitId)
        {
            var unit = unitRepository.FindById(unitId);
            unitRepository.Delete(unit);
            unitOfWork.Save();
        }

        public void Update(int unitId, UpdateUnitDto dto)
        {
            var unit = unitRepository.FindById(unitId);
            unit.Name = dto.Name;
            unit.IsActive = dto.IsActive;
            unitRepository.Update(unit);
            unitOfWork.Save();

        }

    }
}