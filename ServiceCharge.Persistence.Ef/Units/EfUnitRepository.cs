using ServiceCharge.Services.Units.Contracts;

namespace ServiceCharge.Persistence.Ef.Units;

public class EfUnitRepository:UnitRepository
{
    private readonly EfDataContext _context;

    public EfUnitRepository(EfDataContext context)
    {
        _context = context;
    }
}