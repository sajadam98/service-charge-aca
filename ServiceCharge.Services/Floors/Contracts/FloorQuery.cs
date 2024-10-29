using ServiceCharge.Services.Floors.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCharge.Services.Floors.Contracts
{
    public interface FloorQuery
    {
        List<GetAllFloorsDto> GetAll();
    }
}
