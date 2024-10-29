using ServiceCharge.Persistence.Ef;
using ServiceCharge.Persistence.Ef.Units;
using ServiceCharge.Services.Units.Contracts;

namespace ServiceCharge.TestTools.Units;

public static class UnitQueryFactory
{
    public static UnitQuery CreateUnitQuery(EfDataContext context)
    {
        return new EFUnitQuery(context);
    }
}