using ServiceCharge.Entities;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.Floors.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCharge.Persistence.Ef.Floors
{
    public class EFFloorQuery(EfDataContext context) : FloorQuery
    {
        public List<GetAllFloorsDto> GetAll()
        {
            return context.Set<Floor>().Select(f => new GetAllFloorsDto
            {
                Name=f.Name,
                UnitCount=f.UnitCount,
                BlockId=f.BlockId


            }).ToList();
        }
    }
}
