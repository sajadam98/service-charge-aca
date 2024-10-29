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
        var block = new Block
        {
            Name = "Dummy_Block_Name",
            CreationDate = DateTime.Now,
            FloorCount = 2
        };
        Save(block);
        var dto = new AddFloorDto
        {
            UnitCount = 3,
            Name = "Dummy_Name"
        };

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
        var dto = new AddFloorDto
        {
            Name = "Dummy_Name"
        };

        var actual = () => _sut.Add(invalidBlockId, dto);

        actual.Should().ThrowExactly<BlockNotFoundException>();
        ReadContext.Set<Floor>().Should().BeEmpty();
    }

    [Theory]
    [InlineData("Yas")]
    public void
        Add_throw_exception_when_floor_does_exist_with_same_block_with_given_name(
            string name)
    {
        var block = new Block
        {
            Name = "Dummy_Block_Name",
            CreationDate = DateTime.Now,
            FloorCount = 2
        };
        Save(block);
        var floor = new Floor
        {
            Name = name,
            BlockId = block.Id,
            UnitCount = 3
        };
        Save(floor);
        var dto = new AddFloorDto
        {
            Name = name,
            UnitCount = 3
        };

        var actual = () => _sut.Add(block.Id, dto);

        actual.Should().ThrowExactly<DuplicateFloorNameException>();
        ReadContext.Set<Floor>().Should().HaveCount(1)
            .And.ContainSingle(f => f.Id == floor.Id &&
                                    f.UnitCount == floor.UnitCount &&
                                    f.Name == floor.Name &&
                                    f.BlockId == floor.BlockId);
    }

    [Theory]
    [InlineData("SameName")]
    public void Add_floor_to_different_block_with_same_floor_name_properly(
        string sameName)
    {
        var block = new Block()
        {
            Name = "dummy_name",
            CreationDate = DateTime.UtcNow,
            FloorCount = 3
        };
        Save(block);
        var block2 = new Block()
        {
            Name = "dummy_name_1",
            CreationDate = DateTime.UtcNow,
            FloorCount = 3
        };
        Save(block2);
        var floor = new Floor()
        {
            Name = sameName,
            UnitCount = 3,
            BlockId = block.Id
        };
        Save(floor);
        var dto = new AddFloorDto()
        {
            Name = sameName,
            UnitCount = 4,
        };

        var actual = _sut.Add(block2.Id, dto);

        var expected = ReadContext.Set<Floor>();
        expected.Should()
            .Contain(_ =>
                _.Id == actual && _.Name == sameName && _.BlockId == block2.Id);
        expected.Should()
            .Contain(_ =>
                _.Id == floor.Id && _.Name == sameName &&
                _.BlockId == block.Id);
        expected.ToList().Should().HaveCount(2);
    }

    [Fact]
    public void Add_throw_exception_when_block_floor_count_is_full()
    {
        var block = BlockFactory.Create(floorCount:1);
        //     new Block()
        // {
        //     Name = "dummy_name",
        //     CreationDate = DateTime.UtcNow,
        //     FloorCount = 1,
        // };
        Save(block);

        var floor1 = FloorFactory.Create(blockId:block.Id);
        //     new Floor()
        // {
        //     Name = "floor_1",
        //     UnitCount = 2,
        //     BlockId = block.Id
        // };
        Save(floor1);


        var dto = new AddFloorDto()
        {
            Name = "extra_floor",
            UnitCount = 2
        };

        var actual = () => _sut.Add(block.Id, dto);

        actual.Should().ThrowExactly<BlockReachedMaxFloorException>();
        ReadContext.Set<Floor>().Should().HaveCount(1);
    }

    [Theory]
    [InlineData("test_1", 2)]
    [InlineData("test_2", 3)]
    [InlineData("test_3", 4)]
    public void Update_update_floor_properly(
        string newName,
        int newUnitCount)
    {
        var block = BlockFactory.Create();
        Save(block);
        var floor = FloorFactory.Create(blockId: block.Id);
        Save(floor);
        var floor2 = FloorFactory.Create(blockId: block.Id, name: "floor2");
        Save(floor2);
        var dto = new PutFloorDto()
        {
            Id = floor.Id,
            Name = newName,
            UnitCount = newUnitCount
        };
        _sut.Update(dto);

        var excepted = ReadContext.Set<Floor>();
        excepted.Single(_ => _.Id == dto.Id).Should().BeEquivalentTo(new Floor()
        {
            Id = dto.Id,
            Name = dto.Name,
            UnitCount = dto.UnitCount,
            BlockId = block.Id
        });
        excepted.Single(_ => _.Id == floor2.Id).Name.Should().Be(floor2.Name);
        excepted.Single(_ => _.Id == floor2.Id).Id.Should().Be(floor2.Id);
        excepted.Single(_ => _.Id == floor2.Id).UnitCount.Should()
            .Be(floor2.UnitCount);
    }

    [Theory]
    [InlineData("test_1")]
    [InlineData("test_2")]
    public void Update_set_same_floor_name_in_different_block_properly(
        string sameName)
    {
        var block = BlockFactory.Create();
        Save(block);
        var block2 = BlockFactory.Create();
        Save(block2);
        var floor = FloorFactory.Create(blockId: block.Id);
        Save(floor);
        var floor2 = FloorFactory.Create(blockId: block2.Id, name: sameName);
        Save(floor2);
        var dto = new PutFloorDto()
        {
            Id = floor.Id,
            Name = sameName,
            UnitCount = 1
        };

        _sut.Update(dto);

        var excepted = ReadContext.Set<Floor>();
        excepted.Should().HaveCount(2)
            .And.ContainSingle(_ =>
                _.Id == dto.Id && _.BlockId == block.Id && _.Name == sameName)
            .And.ContainSingle(_ =>
                _.Id == floor2.Id && _.BlockId == block2.Id &&
                _.Name == sameName);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void Update_throw_exception_when_floor_not_found(int faKeId)
    {
        var dto = new PutFloorDto()
        {
            Id = faKeId,
            Name = "dummy_floor_name",
            UnitCount = 1
        };


        var actual = () => _sut.Update(dto);

        actual.Should().ThrowExactly<FloorNotFoundException>();
    }

    [Theory]
    [InlineData("test_1")]
    [InlineData("test_2")]
    public void Update_throw_exception_when_floor_name_is_duplicated(
        string sameName)
    {
        var block = BlockFactory.Create();
        Save(block);
        var floor = FloorFactory.Create(blockId: block.Id);
        Save(floor);
        var floor2 = FloorFactory.Create(blockId: block.Id, name: sameName);
        Save(floor2);
        var dto = new PutFloorDto()
        {
            Id = floor.Id,
            Name = sameName,
            UnitCount = 2
        };

        var actual = () => _sut.Update(dto);

        actual.Should().ThrowExactly<DuplicateFloorNameException>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void
        Update_throw_exception_when_unit_count_is_less_than_existence_units(
            int newUnitCount)
    {
        var block = BlockFactory.Create();
        Save(block);
        var floor = FloorFactory.Create(blockId: block.Id, unitCount: 6);
        Save(floor);
        for (int i = 1; i <= 3; i++)
        {
            var unit = UnitFactory.Create(floorId: floor.Id);
            Save(unit);
        }

        var dto = new PutFloorDto()
        {
            Id = floor.Id,
            Name = floor.Name,
            UnitCount = newUnitCount
        };

        var actual = () => _sut.Update(dto);

        actual.Should()
            .ThrowExactly<UnitCountisLessThanExistenceUnitsException>();
        ReadContext.Set<Entities.Unit>().Should().HaveCount(3);
    }

    [Fact]
    public void Delete_delete_floor_by_id_properly()
    {
        var block = BlockFactory.Create();
        Save(block);
        var floor = FloorFactory.Create(blockId: block.Id);
        Save(floor);
        var floor2 = FloorFactory.Create(blockId: block.Id);
        Save(floor2);

        _sut.Delete(floor.Id);

        var excepted = ReadContext.Set<Floor>();
        excepted.Should().HaveCount(1);
        excepted.Should().NotContain(_ => _.Id == floor.Id);
        excepted.Should().Contain(_ => _.Id == floor2.Id);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Delete_throw_exception_when_floor_not_found(int fakeId)
    {
        var actual = () => _sut.Delete(fakeId);

        actual.Should().ThrowExactly<FloorNotFoundException>();
    }
}