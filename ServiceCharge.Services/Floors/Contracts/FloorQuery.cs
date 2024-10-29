using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceCharge.Services.Floors.Contracts.Dto;

namespace ServiceCharge.Services.Floors.Contracts
{
    public interface FloorQuery
    {
        List<GetAllFloorsBlockDto> GetAll(int blockId);
    }
}
