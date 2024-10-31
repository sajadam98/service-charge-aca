using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;
using ServiceCharge.TestTools.Infrastructure.DataBaseConfig.Integration;
using ServiceCharge.TestTools.Units;

namespace ServiceCharge.Service.Unit.Tests.Floors;

public class FloorServiceTests : BusinessIntegrationTest
{
    private readonly FloorService _sut;

    public FloorServiceTests()
    {
        var floorRepository = new EFFloorRepository(SetupContext);
        var blockRepository = new EFBlockRepository(SetupContext);
        var unitOfWork = new EfUnitOfWork(SetupContext);
        _sut = new FloorAppService(floorRepository, blockRepository,
            unitOfWork);
    }

    [Fact]
    public void Add_add_a_floor_properly()
    {
        var block1 = BlockFactory.Create("Block1");
        Save(block1);
        var dto = FloorFactory.AddFloorDto("Name", 5);

        int result = _sut.Add(block1.Id, dto);

        var actual = ReadContext.Set<Floor>().Single();
        actual.Should().BeEquivalentTo
        (FloorFactory
                .Generate(block1.Id, dto.Name, dto.UnitCount),
            _ => _.Excluding(b => b.Id));
    }

    [Theory]
    [InlineData(-1)]
    public void Add_throw_exception_when_block_not_found(
        int invalidBlockId)
    {
        var dto = FloorFactory.AddFloorDto("Name");

        var actual = () => _sut.Add(invalidBlockId, dto);

        actual.Should().Throw<BlockNotFoundException>();
        ReadContext.Set<Floor>().Should().BeEmpty();
    }


    [Theory]
    [InlineData("Yas")]
    public void Add_throw_exception_when_floor_name_duplicated(
        string floorName)
    {
        var block1 = BlockFactory.Create("Block1", 2);
        Save(block1);
        var floor1 = FloorFactory.Generate(block1.Id, floorName);
        Save(floor1);

        var dto = FloorFactory.AddFloorDto(floorName);

        var actual = () => _sut.Add(block1.Id, dto);

        actual.Should().ThrowExactly<FloorNameDuplicateException>();
        ReadContext.Set<Floor>().Should().HaveCount(1)
            .And.ContainSingle(f => f.Id == floor1.Id &&
                                    f.UnitCount == floor1.UnitCount &&
                                    f.Name == floor1.Name &&
                                    f.BlockId == floor1.BlockId);
    }

    [Fact]
    public void Add_throw_exception_when_block_floors_count_is_max()
    {
        var block1 = BlockFactory.Create("Block1", 1);
        Save(block1);
        var floor1 = FloorFactory.Generate(block1.Id);
        Save(floor1);

        var dto = FloorFactory.AddFloorDto("Name", 5);

        var actual = () => _sut.Add(block1.Id, dto);

        actual.Should().ThrowExactly<BlockHasMaxFloorsCountException>();
        ReadContext.Set<Floor>().Should().HaveCount(1)
            .And.ContainSingle(f =>
                f.Id == floor1.Id && f.BlockId == floor1.BlockId);
    }

    [Theory]
    [InlineData("Yas")]
    public void
        Add_add_a_floor_with_same_name_in_different_blocks_properly(
            string floorName)
    {
        var block1 = BlockFactory.Create("Block1", 2);
        Save(block1);
        var block2 = BlockFactory.Create("Block2", 2);
        Save(block2);
        var floor1 = FloorFactory.Generate(block1.Id);
        Save(floor1);

        var dto = FloorFactory.AddFloorDto(floorName);

        var actual = _sut.Add(block2.Id, dto);

        ReadContext.Set<Floor>().Should().HaveCount(2)
            .And.ContainSingle(f =>
                f.Id == floor1.Id && f.BlockId == block1.Id)
            .And.ContainSingle(f =>
                f.Id == actual && f.BlockId == block2.Id);
    }

    [Fact]
    public void Update_update_a_floor_properly()
    {
        var block1 = BlockFactory.Create("Block1");
        Save(block1);
        var floor1 = FloorFactory.Generate(block1.Id, "Floor1");
        Save(floor1);
        var floor2 = FloorFactory.Generate(block1.Id, "Floor2");
        Save(floor2);

        var dto = FloorFactory.UpdateFloorDto("Updated!", 8);

        _sut.Update(floor1.Id, dto);

        var actual = ReadContext.Set<Floor>()
            .FirstOrDefault(_ => _.Id == floor1.Id);

        actual.Should().BeEquivalentTo(dto);
    }

    [Theory]
    [InlineData(-1)]
    public void Update_throw_exception_when_block_id_is_not_found(
        int invalidBlockId)
    {
        var dto = FloorFactory.AddFloorDto("Floor1");

        var actual = () => _sut.Add(invalidBlockId, dto);

        actual.Should().Throw<BlockNotFoundException>();
        ReadContext.Set<Floor>().Should().BeEmpty();
    }

    [Fact]
    public void
        Update_throw_exception_when_floor_unit_count_is_more_than_update_unit_count()
    {
        var block1 = BlockFactory.Create("Block1");
        Save(block1);
        var floor1 = FloorFactory.Generate(block1.Id, "Floor1", 2);
        Save(floor1);
        var unit1 = UnitFactory.Create( floor1.Id);
        Save(unit1);
        var unit2 = UnitFactory.Create(floor1.Id);
        Save(unit2);

        var dto = FloorFactory.UpdateFloorDto("Updated!");

        var actual = () => _sut.Update(floor1.Id, dto);

        actual.Should().ThrowExactly<FloorHasMaxUnitsCountException>();
    }

    [Fact]
    public void Update_throw_exception_when_name_is_duplicate()
    {
        var block = new BlockBuilder()
            .Build();
        Save(block);
        var floor = FloorFactory.Generate(block.Id, "Floor1");
        Save(floor);
        var floor2 = FloorFactory.Generate(block.Id, "Floor2");
        Save(floor2);

        var dto = FloorFactory.UpdateFloorDto(floor2.Name, 60);

        var actual = () => _sut.Update(floor.Id, dto);

        actual.Should().ThrowExactly<FloorNameDuplicateException>();
    }

    [Fact]
    public void Update_updates_floor_when_name_not_changed_properly()
    {
        var block = new BlockBuilder()
            .Build();
        Save(block);
        var floor = FloorFactory.Generate(block.Id, "Floor1");
        Save(floor);
        var floor2 = FloorFactory.Generate(block.Id, "Floor2");
        Save(floor2);

        var dto = FloorFactory.UpdateFloorDto(floor.Name, 60);

        _sut.Update(floor.Id, dto);

        ReadContext.Set<Floor>().Should().ContainSingle(f =>
            f.Id == floor.Id && f.Name == dto.Name &&
            f.UnitCount == dto.UnitCount);
    }

    [Fact]
    public void Delete_delete_a_floor_properly()
    {
        var block1 = BlockFactory.Create("Block1");
        Save(block1);
        var floor1 = FloorFactory.Generate(block1.Id, "Floor1");
        Save(floor1);
        var floor2 = FloorFactory.Generate(block1.Id, "Floor2");
        Save(floor2);

        _sut.Delete(floor2.Id);

        var actual = ReadContext.Set<Floor>().Single();
        actual.Should().BeEquivalentTo(FloorFactory
            .Generate(floor1.BlockId, floor1.Name, floor1.UnitCount), _ =>
            _
                .Excluding(_ => _.Id));
    }

    [Theory]
    [InlineData(-1)]
    public void Delete_throw_exception_if_floor_not_found(
        int invalidFloorId)
    {
        var actual = () => _sut.Delete(invalidFloorId);

        actual.Should().ThrowExactly<FloorNotFoundException>();
        ReadContext.Set<Floor>().Should().BeEmpty();
    }

    [Fact]
    public void Delete_throw_exception_if_floor_has_units()
    {
        var block1 = BlockFactory.Create("Block1");
        Save(block1);
        var floor1 = FloorFactory.Generate(block1.Id);
        Save(floor1);
        var unit1 = UnitFactory.Create( floor1.Id);
        Save(unit1);

        var actual = () => _sut.Delete(floor1.Id);

        actual.Should().ThrowExactly<FloorHasUnitsException>();
    }
}