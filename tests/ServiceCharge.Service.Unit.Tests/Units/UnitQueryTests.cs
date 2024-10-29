using FluentAssertions;
using ServiceCharge.Services.Units.Contracts;
using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;
using ServiceCharge.TestTools.Units;

namespace ServiceCharge.Service.Unit.Tests.Units;

public class UnitQueryTests : BusinessIntegrationTest
{
    private readonly UnitQuery _sut;

    public UnitQueryTests()
    {
        _sut = UnitQueryFactory.CreateUnitQuery(SetupContext);
    }

    [Fact]
    public void GetAll_get_all_units_properly()
    {
        var block = new BlockBuilder().Build();
        Save(block);
        var floor = new FloorBuilder(block.Id).Build();
        Save(floor);
        var unit1 = new UnitBuilder(floor.Id).Build();
        Save(unit1);
        var unit2 = new UnitBuilder(floor.Id).Build();
        Save(unit2);

        var actual = _sut.GetAll();

        actual.Should().HaveCount(2);
        actual.Should().ContainEquivalentOf(new GetAllUnitsDto
        {
            Id = unit1.Id,
            Name = unit1.Name,
            IsActive = unit1.IsActive,
            FloorId = floor.Id,
        });
        actual.Should().ContainEquivalentOf(new GetAllUnitsDto
        {
            Id = unit2.Id,
            Name = unit2.Name,
            IsActive = unit2.IsActive,
            FloorId = floor.Id,
        });
    }
}