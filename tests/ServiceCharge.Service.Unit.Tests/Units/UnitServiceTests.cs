using ServiceCharge.Service.Unit.Tests.Floors.FactoryBuilder;
using ServiceCharge.Service.Unit.Tests.Units.FactoryBuilder;

namespace ServiceCharge.Service.Unit.Tests.Units;

public class UnitServiceTests : BusinessIntegrationTest
{
    private readonly UnitService _sut;

    public UnitServiceTests()
    {
        var unitRepository = new EFUnitRepository(SetupContext);
        var unitOfWork = new EfUnitOfWork(SetupContext);
        var floorRepository = new EFFloorRepository(SetupContext);
        _sut = new UnitAppService(unitRepository, unitOfWork,
            floorRepository);
    }

    [Fact]
    public void Add_add_a_unit_properly()
    {
        var block1 = BlockFactory.Create("Block1");
        Save(block1);
        var floor1 = FloorFactory.Generate(block1.Id, "Floor1", block1.Id);
        Save(floor1);
        var dto = UnitFactory.AddUnitDto("Unit1", true, floor1.Id);

        var result = _sut.Add(floor1.Id, dto);

        var actual = ReadContext.Set<Entities.Units.Unit>().Single();

        actual.Should().BeEquivalentTo(UnitFactory
            .Create(dto.Name, dto.FloorId, dto.IsActive), _ => _
            .Excluding(_ => _.Id));
    }

    [Theory]
    [InlineData(-1)]
    public void Add_throw_exception_when_floor_not_found(
        int invalidFloorId)
    {
        var dto = UnitFactory.AddUnitDto("Unit1");

        var actual = () => _sut.Add(invalidFloorId, dto);

        actual.Should().ThrowExactly<FloorNotFoundException>();
        ReadContext.Set<Entities.Units.Unit>().Should().BeEmpty();
    }

    [Theory]
    [InlineData("Yas")]
    public void Add_throw_exception_when_unit_name_duplicated(
        string unitName)
    {
        var block1 = BlockFactory.Create("Block1", 2);
        Save(block1);
        var floor1 = FloorFactory.Generate(block1.Id, "Floor1", 3);
        Save(floor1);

        var unit1 = UnitFactory.Create(unitName, floor1.Id);
        Save(unit1);

        var dto = UnitFactory.AddUnitDto(unitName, true, floor1.Id);

        var actual = () => _sut.Add(floor1.Id, dto);

        actual.Should().ThrowExactly<UnitNameDuplicateException>();
        ReadContext.Set<Entities.Units.Unit>().Should().HaveCount(1)
            .And.ContainSingle(u => u.Name == unit1.Name &&
                                    u.FloorId == unit1.FloorId &&
                                    u.IsActive == unit1.IsActive);
    }

    [Fact]
    public void Add_throw_exception_when_floor_has_max_number_of_units()
    {
        var block1 = BlockFactory.Create("Block1");
        Save(block1);
        var floor1 = FloorFactory.Generate(block1.Id, "Floor1", 1);
        Save(floor1);
        var unit1 = UnitFactory.Create("Unit1", floor1.Id);
        Save(unit1);

        var dto = UnitFactory.AddUnitDto("Unit2");

        var actual = () => _sut.Add(floor1.Id, dto);

        actual.Should().ThrowExactly<FloorHasMaxUnitsCountException>();
        ReadContext.Set<Entities.Units.Unit>().Should().HaveCount(1)
            .And.ContainSingle(u => u.Id == unit1.Id &&
                                    u.Name == unit1.Name &&
                                    u.IsActive == unit1.IsActive);
    }

    [Fact]
    public void Update_update_a_unit_properly()
    {
        var block1 = BlockFactory.Create("Block1");
        Save(block1);
        var floor1 = FloorFactory.Generate(block1.Id, "Floor1", 2);
        Save(floor1);
        var unit1 = UnitFactory.Create("Unit1", floor1.Id);
        Save(unit1);

        var dto = UnitFactory.UpdateUnitDto("Updated!");

        _sut.Update(unit1.Id, dto);

        var actual = ReadContext.Set<Entities.Units.Unit>()
            .FirstOrDefault();

        actual.Should().BeEquivalentTo(dto);
    }

    [Theory]
    [InlineData(-1)]
    public void Update_throw_exception_when_floor_id_is_not_found(
        int invalidFloorId)
    {
        var dto = UnitFactory.AddUnitDto("Unit1");

        var actual = () => _sut.Add(invalidFloorId, dto);

        actual.Should().Throw<FloorNotFoundException>();
        ReadContext.Set<Entities.Units.Unit>().Should().BeEmpty();
    }

    [Fact]
    public void Delete_delete_a_unit_properly()
    {
        var block1 = BlockFactory.Create("Block1");
        Save(block1);
        var floor1 = FloorFactory.Generate(block1.Id, "Floor1", block1.Id);
        Save(floor1);
        var unit1 = UnitFactory.Create("Unit1", floor1.Id);
        Save(unit1);
        var unit2 = UnitFactory.Create("Unit2", floor1.Id);
        Save(unit2);

        _sut.Delete(unit1.Id);

        var actual = ReadContext.Set<Entities.Units.Unit>().Single();

        actual.Should().BeEquivalentTo(UnitFactory
            .Create(unit2.Name, unit2.FloorId, unit2.IsActive), _ => _
            .Excluding(_ => _.Id));
    }

    [Theory]
    [InlineData(-1)]
    public void Delete_throw_exception_if_unit_does_not_exist(
        int invalidUnitId)
    {
        var actual = () => _sut.Delete(invalidUnitId);

        actual.Should().ThrowExactly<UnitNotFoundException>();
        ReadContext.Set<Entities.Units.Unit>().Should().BeEmpty();
    }
}