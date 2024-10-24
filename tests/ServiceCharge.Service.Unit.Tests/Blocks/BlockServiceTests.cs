
namespace ServiceCharge.Service.Unit.Tests.Blocks;

public class BlockServiceTests : BusinessIntegrationTest
{
    private readonly BlockService _sut;
    private readonly Mock<DateTimeService> _dateTimeServiceMock;

    public BlockServiceTests()
    {
        var repository = new EFBlockRepository(Context);
        _dateTimeServiceMock = new Mock<DateTimeService>();

        _dateTimeServiceMock.Setup(_ => _.NowUtc)
            .Returns(new DateTime(2020, 1, 1));

        var unitOfWork = new EfUnitOfWork(Context);

        _sut = new BlockAppService(
            repository,
            unitOfWork,
            _dateTimeServiceMock.Object);
    }

    [Fact]
    public void Add_add_a_block_properly()
    {
        var dto = new AddBlockDto()
        {
            Name = "dummy",
            FloorCount = 10
        };

        _sut.Add(dto);

        var actual = ReadContext.Set<Block>().Single();
        actual.Should().BeEquivalentTo(new Block()
        {
            Name = dto.Name,
            FloorCount = dto.FloorCount,
            CreationDate = new DateTime(2020, 1, 1),
            Floors = []
        }, _ => _.Excluding(a => a.Id));
    }

    [Fact]
    public void Add_throw_exception_when_block_name_duplicated()
    {
        var block = new Block()
        {
            Name = "CreateFloorWithUnits",
            CreationDate = DateTime.UtcNow
        };
        Save(block);
        var dto = new AddBlockDto()
        {
            Name = "CreateFloorWithUnits"
        };

        var actual = () => _sut.Add(dto);

        actual.Should().ThrowExactly<BlockNameDuplicateException>();
    }

    [Fact]
    public void AddWithFloor_create_a_block_with_floors_properly()
    {
        var dto = new AddBlockWithFloorDto()
        {
            Name = "block1",
            Floors =
            [
                new()
                {
                    Name = "floor1",
                    UnitCount = 2
                }
            ]
        };

        _sut.AddWithFloor(dto);

        var actual = ReadContext.Set<Block>()
            .Include(_ => _.Floors)
            .Single();
        actual.Name.Should().Be("block1");
        actual.FloorCount.Should().Be(1);
        actual.Floors.Should().HaveCount(1);
        actual.Floors.Should()
            .Contain(_ => _.Name == "floor1" && _.UnitCount == 2);
    }

    [Fact]
    public void AddWithFloor_throw_exception_when_block_name_duplicated()
    {
        var block = new Block()
        {
            Name = "block1",
            CreationDate = DateTime.UtcNow
        };
        Save(block);
        var dto = new AddBlockWithFloorDto()
        {
            Name = "block1",
            Floors =
            [
                new()
                {
                    Name = "floor1",
                    UnitCount = 2
                }
            ]
        };

        var actual = () => _sut.AddWithFloor(dto);

        actual.Should().ThrowExactly<BlockNameDuplicateException>();
    }

    [Fact]
    public void Update_update_a_block_properly()
    {
        var block = new Block()
        {
            Name = "CreateFloorWithUnits",
            CreationDate = DateTime.UtcNow.AddDays(-1)
        };
        Save(block);
        var dto = new PutBlockDto()
        {
            Id = block.Id,
            Name = "dummy",
            CreationDate = new DateTime(2024, 10, 20),
            FloorCount = 4
        };

        _sut.Update(dto);

        var actuall = ReadContext.Set<Block>().Single();
        actuall.Should().BeEquivalentTo(new Block()
        {
            Id = block.Id,
            Name = dto.Name,
            CreationDate = dto.CreationDate.Value,
            FloorCount = dto.FloorCount.Value,
        });
    }

    [Fact]
    public void Update_throw_exception_when_name_duplicated()
    {
        var block = new Block()
        {
            Name = "name1",
            CreationDate = DateTime.UtcNow.AddDays(-1)
        };
        Save(block);
        var block2 = new Block()
        {
            Name = "name2",
            CreationDate = DateTime.UtcNow.AddDays(-2)
        };
        Save(block2);
        var dto = new PutBlockDto()
        {
            Id = block.Id,
            Name = block2.Name,
        };

        var actual = () => _sut.Update(dto);

        actual.Should().ThrowExactly<BlockNameDuplicateException>();
    }

    [Fact]
    public void Update_throw_exception_when_block_not_found()
    {
        var blockId = -1;
        var dto = new PutBlockDto()
        {
            Id = blockId
        };
        var actual = () => _sut.Update(dto);
        actual.Should().ThrowExactly<BlockNotFoundException>();
    }

    // [Fact]
    // public void
    //     Update_throw_exception_when_floorCount_is_less_than_added_floors()
    // {
    //
    //     var actual = () => _sut.Update(dto);
    //     actual.Should().ThrowExactly<>();
    // }

    
}