using ServiceCharge.Persistence.Ef.Units;
using ServiceCharge.Services.Units.Contracts;

namespace ServiceCharge.Service.Unit.Tests.Units;

public class UnitQueryTest : BusinessIntegrationTest
{
    private readonly UnitQuery _sut;

    public UnitQueryTest()
    {
        _sut = new EfUnitQuery(Context);
    }

    [Fact]
    public void
        GetAllUnitsWithFloorAndBlock_get_all_units_with_floor_name_and_block_name_properly()
    {
        
    }
}