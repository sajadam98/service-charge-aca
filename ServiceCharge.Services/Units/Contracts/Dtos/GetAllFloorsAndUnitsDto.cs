using System.Reflection.Metadata.Ecma335;
using ServiceCharge.Entities;

namespace ServiceCharge.Services.Units.Contracts.Dtos;

public class GetAllFloorsAndUnitsDto
{
    public required string FloorName { get; set; }
    public required string BlockName { get; set; }

    public required string UnitName { get; set; }

    public bool IsActive { get; set; }
   
    
}