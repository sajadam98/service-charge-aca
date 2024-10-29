using ServiceCharge.Persistence.Ef;
using ServiceCharge.Persistence.Ef.Floors;
using ServiceCharge.Services.Floors.Contracts;

namespace ServiceCharge.TestTools.Floors;

public static class FloorQueryFactory
{
    public static FloorQuery CreateQuery(EfDataContext context)
    {
        return new EFFloorQuery(context);
    }
}