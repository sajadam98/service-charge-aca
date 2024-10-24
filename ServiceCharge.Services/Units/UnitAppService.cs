using ServiceCharge.Services.Units.Contracts;

namespace ServiceCharge.Services.Units;

public class UnitAppService:UnitsService
{
    private readonly UnitOfWork _context;
    private readonly UnitRepository _repository;

    public UnitAppService(UnitOfWork context, UnitRepository repository)
    {
        _context = context;
        _repository = repository;
    }
}