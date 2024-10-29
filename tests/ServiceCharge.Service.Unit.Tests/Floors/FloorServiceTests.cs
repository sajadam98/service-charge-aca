using FluentAssertions;
using ServiceCharge.Entities;
using ServiceCharge.Services.Blocks.Exceptions;
using ServiceCharge.Services.Floors;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.Floors.Exceptions;
using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;
using ServiceCharge.TestTools.Floors.DTOs;
using ServiceCharge.TestTools.Units;

namespace ServiceCharge.Service.Unit.Tests.Floors;

public class FloorServiceTests : BusinessIntegrationTest
{
    private readonly FloorService _sut;

    public FloorServiceTests()
    {
        _sut = FloorServiceFactory.CreateService(SetupContext);
    }

    [Fact]
    public void Add_adds_floor_properly()
    {
        var block = new BlockBuilder()
            .WithFloorCount(1)
            .Build();
        Save(block);
        var dto = AddFloorDtoFactory.CreateDto();

        var actual = _sut.Add(block.Id, dto);

        var expected =
            ReadContext.Set<Floor>().Single(f => f.Id == actual);
        expected.Name.Should().Be(dto.Name);
        expected.UnitCount.Should().Be(dto.UnitCount);
        expected.BlockId.Should().Be(block.Id);
    }

    [Theory]
    [InlineData(-1)]
    public void Add_throw_exception_when_block_not_exist_exception(
        int invalidBlockId)
    {
        var dto = AddFloorDtoFactory.CreateDto();

        var actual = () => _sut.Add(invalidBlockId, dto);

        actual.Should().ThrowExactly<BlockNotFoundException>();
        ReadContext.Set<Floor>().Should().BeEmpty();
    }

    [Theory]
    [InlineData("Yas")]
    public void
        Add_throw_exception_when_floor_does_exist_in_same_block_with_given_name(
            string floorName)
    {
        var block = new BlockBuilder()
            .WithFloorCount(2)
            .Build();
        Save(block);
        var floor = new FloorBuilder(block.Id).WithName(floorName).Build();
        Save(floor);
        var dto = AddFloorDtoFactory.CreateDto(floorName: floorName);

        var actual = () => _sut.Add(block.Id, dto);

        actual.Should().ThrowExactly<DuplicateFloorNameException>();
        ReadContext.Set<Floor>().Should().HaveCount(1)
            .And.ContainSingle(f => f.Id == floor.Id &&
                                    f.UnitCount == floor.UnitCount &&
                                    f.Name == floor.Name &&
                                    f.BlockId == floor.BlockId);
    }

    [Theory]
    [InlineData("Yas")]
    public void Add_adds_floor_with_same_name_in_different_blocks_properly(
        string floorName)
    {
        var block1 = new BlockBuilder().Build();
        Save(block1);
        var block2 = new BlockBuilder().Build();
        Save(block2);
        var floor = FloorFactory.CreateFloor(block1.Id, floorName);
        Save(floor);
        var dto = AddFloorDtoFactory.CreateDto(floorName: floorName);

        var actual = _sut.Add(block2.Id, dto);

        ReadContext.Set<Floor>().Should().HaveCount(2)
            .And.ContainSingle(f =>
                f.Id == floor.Id && f.BlockId == block1.Id)
            .And.ContainSingle(f =>
                f.Id == actual && f.BlockId == block2.Id);
    }

    [Fact]
    public void Add_throw_exception_when_blocks_floor_capacity_fulled()
    {
        var block = new BlockBuilder()
            .WithFloorCount(1)
            .Build();
        Save(block);
        var floor = new FloorBuilder(block.Id).Build();
        Save(floor);
        var dto = AddFloorDtoFactory.CreateDto();

        var actual = () => _sut.Add(block.Id, dto);

        actual.Should().ThrowExactly<BlockFloorsCapacityFulledException>();
        ReadContext.Set<Floor>().Should().HaveCount(1)
            .And.ContainSingle(f =>
                f.Id == floor.Id && f.BlockId == floor.BlockId);
    }

    [Fact]
    public void Update_update_a_floor_properly()
    {
        var block = new BlockBuilder().Build();
        Save(block);
        var floor = new FloorBuilder(block.Id).Build();
        Save(floor);
        var dto = new UpdateFloorDto
        {
            Name = "Edittet",
            UnitCount = 15
        };

        _sut.Update(floor.Id, dto);

        var expected = ReadContext.Set<Floor>().Single();
        expected.Name.Should().Be(dto.Name);
        expected.UnitCount.Should().Be(dto.UnitCount);
    }

    [Theory]
    [InlineData(-1)]
    public void Update_throw_exception_when_floor_not_exist(int dummyId)
    {
        var dto = new UpdateFloorDto
        {
            Name = "Edittet",
        };

        var actaul = () => _sut.Update(dummyId, dto);

        actaul.Should().ThrowExactly<FloorNotFoundException>();
    }

    [Theory]
    [InlineData("Yas")]
    public void
        Update_throw_exception_when_floor_does_exist_in_same_block_with_given_name(
            string floorName)
    {
        var block = new BlockBuilder().Build();
        Save(block);
        var floor = new FloorBuilder(block.Id).WithName(floorName).Build();
        Save(floor);
        var floor2 = new FloorBuilder(block.Id).Build();
        Save(floor2);
        var dto = new UpdateFloorDto
        {
            Name = floorName,
            UnitCount = 14
        };

        var actual = () => _sut.Update(floor2.Id, dto);

        actual.Should().ThrowExactly<DuplicateFloorNameException>();
    }

    [Fact]
    public void
        Update_throw_exception_when_existing_units_count_is_more_than_new_units_count()
    {
        var block = new BlockBuilder().Build();
        Save(block);
        var floor = new FloorBuilder(block.Id).WithUnitCount(2).Build();
        Save(floor);
        var unit1 = new UnitBuilder(floor.Id).Build();
        Save(unit1);
        var unit2 = new UnitBuilder(floor.Id).Build();
        Save(unit2);
        var dto = new UpdateFloorDto
        {
            Name = "Edittet",
            UnitCount = 1
        };

        var actual = () => _sut.Update(floor.Id, dto);

        actual.Should().ThrowExactly<FloorUnitsCountException>();
    }

    [Fact]
    public void Delete_delete_a_floor_properly()
    {
        var block = new BlockBuilder().Build();
        Save(block);
        var floor1 = new FloorBuilder(block.Id).Build();
        Save(floor1);
        var floor2 = new FloorBuilder(block.Id).Build();
        Save(floor2);

        _sut.Delete(floor1.Id);

        var expected = ReadContext.Set<Floor>().Single();
        expected.Id.Should().Be(floor2.Id);
    }

    [Theory]
    [InlineData(-1)]
    public void Delete_throw_exception_when_floor_does_not_exist(int dummyId)
    {
        var actual = () => _sut.Delete(dummyId);

        actual.Should().ThrowExactly<FloorNotFoundException>();
    }

    [Fact]
    public void Delete_throw_exception_when_floor_has_units()
    {
        var block = new BlockBuilder().Build();
        Save(block);
        var floor = new FloorBuilder(block.Id).Build();
        Save(floor);
        var unit = new Entities.Unit
        {
            Name = "Unit",
            FloorId = floor.Id,
            IsActive = true,
        };
        Save(unit);

        var actual = () => _sut.Delete(floor.Id);

        actual.Should().ThrowExactly<FloorUnitsCountException>();
    }
}