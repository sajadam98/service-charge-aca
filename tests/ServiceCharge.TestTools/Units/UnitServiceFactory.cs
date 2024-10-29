using ServiceCharge.Persistence.Ef;
using ServiceCharge.Persistence.Ef.Blocks;
using ServiceCharge.Persistence.Ef.Floors;
using ServiceCharge.Persistence.Ef.UnitOfWorks;
using ServiceCharge.Persistence.Ef.Units;
using ServiceCharge.Services.Floors;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.Units;
using ServiceCharge.Services.Units.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCharge.TestTools.Units
{
    public class UnitServiceFactory
    {
        public static UnitService CreateService(EfDataContext context, UnitRepository? repository = null)
        {
            repository ??= new EFUnitRepository(context);
           
            var floorRepository = new EFFloorRepository(context);
            var unitRepository = new EFUnitRepository(context);
            var unitOfWork = new EfUnitOfWork(context);
            return new UnitAppService(
               
                floorRepository,
                unitRepository,
                unitOfWork);
        }

    }
}
