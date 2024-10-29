using FluentAssertions;
using ServiceCharge.Services.Floors;
using ServiceCharge.Services.Floors.Exceptions;
using ServiceCharge.Services.Units.Contracts;
using ServiceCharge.Services.Units.Exceptions;
using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;
using ServiceCharge.TestTools.Units;

namespace ServiceCharge.Service.Unit.Tests.Units;

public class UnitServiceTests : BusinessIntegrationTest
{
    private readonly UnitService _sut;

    public UnitServiceTests()
    {
        _sut = UnitServiceFactory.CreateUnitService(SetupContext);
    }

    [Fact]
    public void Add_add_unit_properly()
    {
        var block = new BlockBuilder().Build();
        Save(block);
        var floor = FloorFactory.CreateFloor(block.Id);
        Save(floor);
        var dto = new AddUnitDto
        {
            Name = "Test",
            IsActive = true,
        };

        var actual = _sut.Add(floor.Id, dto);

        var expected = ReadContext.Set<Entities.Unit>().Single();
        expected.Should().BeEquivalentTo(
            new UnitBuilder(floor.Id)
                .WithName(dto.Name)
                .WithId(actual)
                .WithIsActive(dto.IsActive).Build());
    }

    [Theory]
    [InlineData(-1)]
    public void Add_throws_exception_if_floor_dose_not_exists(int dummyId)
    {
        var actual = () => _sut.Add(dummyId, new AddUnitDto { Name = "Test" });

        actual.Should().ThrowExactly<FloorNotFoundException>();
        ReadContext.Set<Entities.Unit>().Should().BeEmpty();
    }

    [Fact]
    public void Add_throws_exception_if_floor_units_count_is_full()
    {
        var block = new BlockBuilder().Build();
        Save(block);
        var floor = FloorFactory.CreateFloor(block.Id, unitCount: 1);
        Save(floor);
        var unit = new UnitBuilder(floor.Id).Build();
        Save(unit);
        var dto = new AddUnitDto
        {
            Name = "Test",
            IsActive = true,
        };

        var actual = () => _sut.Add(floor.Id, dto);

        actual.Should().ThrowExactly<FloorUnitsCountException>();
        ReadContext.Set<Entities.Unit>().Should().HaveCount(1).And
            .ContainEquivalentOf(
                new UnitBuilder(floor.Id)
                    .WithId(unit.Id)
                    .Build());
    }

    [Fact]
    public void Update_update_unit_properly()
    {
        var block = new BlockBuilder().Build();
        Save(block);
        var floor = FloorFactory.CreateFloor(block.Id);
        Save(floor);
        var unit1 = new UnitBuilder(floor.Id).Build();
        Save(unit1);
        var unit2 = new UnitBuilder(floor.Id).Build();
        Save(unit2);
        var dto = new UpdateUnitDto
        {
            Name = "Editted"
        };

        _sut.Update(unit2.Id, dto);

        var expected = ReadContext.Set<Entities.Unit>().ToList();
        expected.Should().HaveCount(2);
        expected.Should()
            .ContainEquivalentOf(
                new UnitBuilder(floor.Id)
                    .WithId(unit1.Id)
                    .Build());
        expected.Should()
            .ContainEquivalentOf(
                new UnitBuilder(floor.Id)
                    .WithId(unit2.Id)
                    .WithName(dto.Name)
                    .Build());
    }

    [Theory]
    [InlineData(-1)]
    public void Update_throws_exception_if_unit_does_not_exists(int dummyId)
    {
        var actual = () =>
            _sut.Update(dummyId, new UpdateUnitDto { Name = "Test" });

        actual.Should().ThrowExactly<UnitNotFoundException>();
    }

    [Fact]
    public void Delete_delete_a_unit_properly()
    {
        var block = new BlockBuilder().Build();
        Save(block);
        var floor = FloorFactory.CreateFloor(block.Id);
        Save(floor);
        var unit1 = new UnitBuilder(floor.Id).Build();
        Save(unit1);
        var unit2 = new UnitBuilder(floor.Id).Build();
        Save(unit2);

        _sut.Delete(unit2.Id);

        var expected = ReadContext.Set<Entities.Unit>().ToList();
        expected.Should().HaveCount(1);
        expected.Should()
            .ContainEquivalentOf(
                new UnitBuilder(floor.Id)
                    .WithId(unit1.Id)
                    .Build());
    }

    [Theory]
    [InlineData(-1)]
    public void Delete_throw_exception_if_unit_does_not_exists(int dummyId)
    {
        var actual = () => _sut.Delete(dummyId);

        actual.Should().ThrowExactly<UnitNotFoundException>();
    }
}