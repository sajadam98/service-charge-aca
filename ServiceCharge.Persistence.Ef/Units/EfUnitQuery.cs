using ServiceCharge.Services.Units.Contracts;

namespace ServiceCharge.Persistence.Ef.Units;

public class EfUnitQuery:UnitQuery
{
    private readonly EfDataContext _context;

    public EfUnitQuery(EfDataContext context)
    {
        _context = context;
    }
}