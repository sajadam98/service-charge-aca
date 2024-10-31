using ServiceCharge.Application.Floors.Handlers.UpdateFloors.Contracts.DTOs;
using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;
using ServiceCharge.TestTools.Infrastructure.DataBaseConfig.Integration;
using ServiceCharge.TestTools.Units;

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
        var dto = UnitFactory.AddUnitDto("Unit1");

        _sut.Add(floor1.Id, dto);

        var actual = ReadContext.Set<Entities.Units.Unit>().Single();

        actual.Should().BeEquivalentTo(UnitFactory
            .Create(floor1.Id, dto.Name, dto.IsActive), o => o
            .Excluding(u => u.Id));
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

        var unit1 = UnitFactory.Create(floor1.Id, unitName);
        Save(unit1);

        var dto = UnitFactory.AddUnitDto(unitName);

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
        var floor1 = FloorFactory.Generate(block1.Id, "Floor1");
        Save(floor1);
        var unit1 = UnitFactory.Create(floor1.Id);
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
        var unit1 = UnitFactory.Create(floor1.Id);
        Save(unit1);

        var dto = UnitFactory.UpdateUnitDto("Updated!");

        _sut.Update(unit1.Id, dto);

        var actual = ReadContext.Set<Entities.Units.Unit>()
            .FirstOrDefault();

        actual.Should().BeEquivalentTo(new Entities.Units.Unit
        {
            Id = unit1.Id,
            Name = dto.Name,
            IsActive = dto.IsActive,
            FloorId = dto.FloorId,
        });
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
        var unit1 = UnitFactory.Create(floor1.Id);
        Save(unit1);
        var unit2 = UnitFactory.Create(floor1.Id);
        Save(unit2);

        _sut.Delete(unit1.Id);

        var actual = ReadContext.Set<Entities.Units.Unit>().Single();

        actual.Should().BeEquivalentTo(UnitFactory
            .Create(unit2.FloorId, unit2.Name, unit2.IsActive), o => o
            .Excluding(u => u.Id));
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

    [Fact]
    public void UpdateRange_update_a_list_of_units_from_a_floor_properly()
    {
        var block1 = BlockFactory.Create();
        Save(block1);
        var floor1 = FloorFactory.Generate(block1.Id);
        Save(floor1);
        var unit1 = UnitFactory.Create(floor1.Id);
        Save(unit1);
        var unit2 = UnitFactory.Create(floor1.Id);
        Save(unit2);
        var unit3 = UnitFactory.Create(floor1.Id);
        Save(unit3);
        var updateDTOs = new HashSet<UpdateUnitsOfFloorDto>();
        var dto1 = new UpdateUnitsOfFloorDto
        {
            Name = "Updated1",
            UnitCount = 80,
            Id = unit1.Id,
            IsActive = false
        };
        var dto2 = new UpdateUnitsOfFloorDto
        {
            Name = "Updated2",
            UnitCount = 25,
            Id = unit3.Id,
            IsActive = true
        };
        updateDTOs.Add(dto1);
        updateDTOs.Add(dto2);

        _sut.UpdateRange(floor1.Id, updateDTOs);

        var expected = ReadContext.Set<Entities.Units.Unit>().ToList();
        expected.Should().HaveCount(3);
        expected.Should().ContainEquivalentOf(new Entities.Units.Unit
        {
            Id = unit1.Id,
            Name = dto1.Name,
            IsActive = dto1.IsActive,
            FloorId = floor1.Id
        });
        expected.Should().ContainEquivalentOf(unit2);
        expected.Should().ContainEquivalentOf(new Entities.Units.Unit
        {
            Id = unit3.Id,
            Name = dto2.Name,
            IsActive = dto2.IsActive,
            FloorId = floor1.Id
        });
    }

    [Theory]
    [InlineData(-1)]
    public void
        UpdateRange_throw_exception_if_one_or_more_units_dosenot_exists(
            int dummyId)
    {
        var block1 = BlockFactory.Create();
        Save(block1);
        var floor1 = FloorFactory.Generate(block1.Id);
        Save(floor1);
        var unit1 = UnitFactory.Create(floor1.Id);
        Save(unit1);
        var updateDTOs = new HashSet<UpdateUnitsOfFloorDto>();
        var dto1 = new UpdateUnitsOfFloorDto
        {
            Name = "Updated1",
            UnitCount = 80,
            Id = unit1.Id,
            IsActive = false
        };
        var dto2 = new UpdateUnitsOfFloorDto
        {
            Name = "Updated2",
            UnitCount = 25,
            Id = dummyId,
            IsActive = true
        };
        updateDTOs.Add(dto1);
        updateDTOs.Add(dto2);

        var actual = () => _sut.UpdateRange(floor1.Id, updateDTOs);
        actual.Should().ThrowExactly<OneOrMoreUnitNotFoundException>();
        var expected = ReadContext.Set<Entities.Units.Unit>().ToList();
        expected.Should().HaveCount(1);
        expected.Should().ContainEquivalentOf(unit1);
    }

    [Fact]
    public void
        UpdateRange_throw_exception_when_unit_dose_not_belong_to_a_floor()
    {
        var block1 = BlockFactory.Create();
        Save(block1);
        var floor1 = FloorFactory.Generate(block1.Id);
        Save(floor1);
        var floor2 = FloorFactory.Generate(block1.Id);
        Save(floor2);
        var unit1 = UnitFactory.Create(floor1.Id);
        Save(unit1);
        var unit2 = UnitFactory.Create(floor1.Id);
        Save(unit2);
        var unit3 = UnitFactory.Create(floor2.Id);
        Save(unit3);
        var updateDTOs = new HashSet<UpdateUnitsOfFloorDto>();
        var dto1 = new UpdateUnitsOfFloorDto
        {
            Name = "Updated1",
            UnitCount = 80,
            Id = unit1.Id,
            IsActive = false
        };
        var dto2 = new UpdateUnitsOfFloorDto
        {
            Name = "Updated2",
            UnitCount = 25,
            Id = unit3.Id,
            IsActive = true
        };
        updateDTOs.Add(dto1);
        updateDTOs.Add(dto2);

        var actual = () => _sut.UpdateRange(floor1.Id, updateDTOs);
        actual.Should().ThrowExactly<OneOrMoreUnitNotFoundException>();
        var expected = ReadContext.Set<Entities.Units.Unit>().ToList();
        expected.Should().HaveCount(3);
        expected.Should().ContainEquivalentOf(unit1);
        expected.Should().ContainEquivalentOf(unit2);
        expected.Should().ContainEquivalentOf(unit3);
    }
}