namespace ServiceCharge.Service.Unit.Tests.Blocks;

public class BlockServiceTests : BusinessIntegrationTest
{
    private readonly BlockService _sut;
    private readonly Mock<DateTimeService> _dateTimeServiceMock;

    public BlockServiceTests()
    {
        var repository = new EFBlockRepository(SetupContext);
        _dateTimeServiceMock = new Mock<DateTimeService>();
        _dateTimeServiceMock.Setup(_ => _.NowUtc)
            .Returns(new DateTime(2022, 1, 1));

        var unitOfWork = new EfUnitOfWork(SetupContext);

        _sut = new BlockAppService(
            repository,
            unitOfWork,
            _dateTimeServiceMock.Object);
    }

    [Fact]
    public void Add_add_a_block_properly()
    {
        var dto = BlockFactory.AddBlockDto("dummy", 10);

        _sut.Add(dto);

        var actual = ReadContext.Set<Block>().Single();
        actual.Should().BeEquivalentTo(BlockFactory
            .Create(dto.Name, dto.FloorCount),_=>_
            .Excluding(b=>b.Id));
    }

    [Theory]
    [InlineData("Yas")]
    public void Add_throw_exception_when_block_name_duplicated(string blockName)
    {
        var block = BlockFactory.Create(blockName, 2);
        Save(block);
        var dto = BlockFactory.AddBlockDto(blockName, 2);

        var actual = () => _sut.Add(dto);

        actual.Should().ThrowExactly<BlockNameDuplicateException>();
        ReadContext.Set<Block>().Should().ContainSingle(b => b.Name == block.Name &&
                                                             b.FloorCount == block.FloorCount);
    }

    [Fact]
    public void AddWithFloor_add_a_block_with_a_floor_properly()
    {
        var dto = BlockFactory.AddBlockWithFloorDto("block1","floor1");

        _sut.AddWithFloor(dto);

        var actual = ReadContext.Set<Block>()
            .Include(_ => _.Floors)
            .Single();
        actual.Name.Should().Be("block1");
        actual.FloorCount.Should().Be(1);
        actual.Floors.Should().HaveCount(1);
        actual.Floors.Should().Contain(_ => _.Name == "floor1" && _.UnitCount == 2);
    }

    [Fact]
    public void Update_update_a_block_properly()
    {
        var block1 = BlockFactory.Create("block1", 2);
        Save(block1);
        var block2 = BlockFactory.Create("block2", 2);
        Save(block2);
        var dto = BlockFactory.UpdateBlockDto("block1updated!");
        
        _sut.Update(block2.Id, dto);
        
        var actual = ReadContext.Set<Block>().FirstOrDefault(_ => _.Id == block2.Id);
        actual.Should().BeEquivalentTo(dto);
    }

    [Theory]
    [InlineData("Yas")]
    public void Update_throw_exception_when_block_name_duplicated(string blockName)
    {
        var block1 = BlockFactory.Create(blockName, 2);
        Save(block1);
        
        var block2 = BlockFactory.Create("block2", 2);
        Save(block2);

        var dto = BlockFactory.UpdateBlockDto(blockName);
        
        var actual = () => _sut.Update(block2.Id, dto);
        actual.Should().ThrowExactly<BlockNameDuplicateException>();
        ReadContext.Set<Block>().Should().ContainSingle(b => b.Name == block1.Name &&
                                                             b.FloorCount == block1.FloorCount);
    }

    [Theory]
    [InlineData(-1)]
    public void Update_throw_exception_when_block_not_found(int invalidBlockId)
    {
        var dto = BlockFactory.UpdateBlockDto("block1");
        var actual  = () => _sut.Update(invalidBlockId,dto);
        
        actual.Should().ThrowExactly<BlockNotFoundException>();
        ReadContext.Set<Block>().Should().BeEmpty();
    }

    [Fact]
    public void Delete_delete_a_block_properly()
    {
        var block1 = BlockFactory.Create("block1", 2);
        Save(block1);
        var block2 = BlockFactory.Create("block2", 2);
        Save(block2);
        
        _sut.Delete(block2.Id);
        
        var actual = ReadContext.Set<Block>().ToHashSet();
        actual.Should().HaveCount(1);
        actual.Should().Contain(_ => _.Id == block1.Id);
        actual.Should().NotContain(_ => _.Id == block2.Id);
    }

    [Theory]
    [InlineData(-1)]
    public void Delete_throw_exception_when_block_not_found(int invalidBlockId)
    {
        var actual  = () => _sut.Delete(invalidBlockId);
        
        actual.Should().ThrowExactly<BlockNotFoundException>();
        ReadContext.Set<Block>().Should().BeEmpty();
    }
}