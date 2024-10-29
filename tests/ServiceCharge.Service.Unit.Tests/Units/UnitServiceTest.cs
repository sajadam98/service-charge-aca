namespace ServiceCharge.Service.Unit.Tests.Units;

public class UnitServiceTest : BusinessIntegrationTest
{
    private readonly UnitService _sut;

    public UnitServiceTest()
    {
        _sut = UnitServiceFactory.CreateService(SetupContext);
    }

    [Fact]
    public void Add_create_unit_properly()
    {
        var block = BlockFactory.Create();
        Save(block);
        var floor = FloorFactory.Create(blockId: block.Id);
        Save(floor);
        var dto = new AddUnitDto()
        {
            Name = "dummy_unit_name",
            IsActive = true
        };

        var actual = _sut.Add(floor.Id, dto);

        var excepted = ReadContext.Set<Entities.Unit>();
        excepted.Should().HaveCount(1)
            .And.ContainSingle(_ => _.Id == actual && _.FloorId == floor.Id);
        excepted.Single().Should().BeEquivalentTo(new Entities.Unit()
        {
            FloorId = floor.Id,
            Name = dto.Name,
            IsActive = dto.IsActive
        }, _ => _.Excluding(_ => _.Id));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Add_throw_exception_when_floor_not_found(int fakeFloorId)
    {
        var dto = new AddUnitDto()
        {
            Name = "dummy_unit_name",
            IsActive = true
        };

        var actual = () => _sut.Add(fakeFloorId, dto);

        actual.Should().ThrowExactly<FloorNotFoundException>();
    }

    [Fact]
    public void Add_throw_exception_when_floor_unit_count_is_full()
    {
        var block = BlockFactory.Create();
        Save(block);
        var floor = FloorFactory.Create(blockId: block.Id, unitCount: 1);
        Save(floor);
        var unit = UnitFactory.Create(floorId: floor.Id);
        Save(unit);
        var dto = new AddUnitDto()
        {
            Name = "dummy",
            IsActive = true
        };

        var actual = () => _sut.Add(floor.Id, dto);

        actual.Should().ThrowExactly<FloorReachedMaxUnitException>();
    }
    
    
}