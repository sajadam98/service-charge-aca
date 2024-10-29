using ServiceCharge.Entities;

namespace ServiceCharge.Services.Floors.Contracts;

public class FindByIdWithUnitsCountDto
{
    public Floor? Floor { get; set; }
    public int ExistingUnitsCount { get; set; }
}