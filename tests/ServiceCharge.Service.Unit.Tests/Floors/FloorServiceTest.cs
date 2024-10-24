using ServiceCharge.Persistence.Ef.Floors;
using ServiceCharge.Services.Floors;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.Floors.Exceptions;
using ServiceCharge.Services.UnitOfWorks;

namespace ServiceCharge.Service.Unit.Tests.Floors;

public class FloorServiceTest : BusinessIntegrationTest
{
    private readonly FloorService _sut;

    public FloorServiceTest()
    {
        var unitOfWork = new EfUnitOfWork(Context);
        var floorRepository = new EfFloorRepository(Context);
        var blockRepository = new EFBlockRepository(Context);
        _sut = new FloorAppService(unitOfWork, floorRepository,
            blockRepository);
    }

    [Fact]
    public void Create_create_a_floor_properly()
    {
        var block = new Block()
        {
            Name = "CreateFloorWithUnits",
            FloorCount = 4,
            CreationDate = new DateTime(2024, 2, 2)
        };
        Save(block);
        var dto = new AddFloorDto()
        {
            Name = "dummy",
            UnitCount = 2
        };

        int result = _sut.Create(block.Id, dto);

        var actual = ReadContext.Set<Floor>().Single();
        actual.Id.Should().Be(result);
        actual.Name.Should().Be(dto.Name);
        actual.UnitCount.Should().Be(dto.UnitCount);
        actual.BlockId.Should().Be(block.Id);
    }

    [Fact]
    public void Create_throw_exception_when_block_not_found()
    {
        var blocId = -1;
        var dto = new AddFloorDto()
        {
            Name = "dummy",
            UnitCount = 2
        };

        var actual = () => _sut.Create(blocId, dto);
        actual.Should().ThrowExactly<BlockNotFoundException>();
    }

    [Fact]
    public void Create_throw_exception_when_floor_name_duplicated_in_block()
    {
        var block = new Block()
        {
            Name = "CreateFloorWithUnits",
            FloorCount = 4,
            CreationDate = new DateTime(2024, 2, 2),
            Floors =
            [
                new Floor()
                {
                    Name = "dummy",
                    UnitCount = 2
                }
            ]
        };
        Save(block);
        var dto = new AddFloorDto()
        {
            Name = "dummy",
            UnitCount = 2
        };

        var actual = () => _sut.Create(block.Id, dto);

        actual.Should().ThrowExactly<FloorNameDuplicateInBlockException>();
    }

    [Fact]
    public void Create_throw_exception_When_Exceeds_Floor_Limit()
    {
        var block = new Block()
        {
            Name = "CreateFloorWithUnits",
            FloorCount = 2,
            CreationDate = new DateTime(2024, 2, 2),
            Floors =
            [
                new Floor()
                {
                    Name = "dummy",
                    UnitCount = 2
                },
                new Floor()
                {
                    Name = "dummy2",
                    UnitCount = 2
                },
            ]
        };
        Save(block);
        var dto = new AddFloorDto()
        {
            Name = "dummy3",
            UnitCount = 2
        };

        var actual = () => _sut.Create(block.Id, dto);
        actual.Should().ThrowExactly<ExceedsFloorLimitException>();
    }

    [Fact]
    public void Update_update_a_floor_properly()
    {
        var block = new Block()
        {
            Name = "CreateFloorWithUnits",
            CreationDate = DateTime.UtcNow,
            FloorCount = 2
        };
        Save(block);
        var floor = new Floor()
        {
            Name = "CreateFloorWithUnits",
            UnitCount = 4,
            BlockId = block.Id
        };
        Save(floor);
        var dto = new PutFloorDto()
        {
            Id = floor.Id,
            Name = "dummy"
        };

        _sut.Update(dto);
        var actual = ReadContext.Set<Floor>().Single();
        actual.Name.Should()
            .Be(string.IsNullOrEmpty(dto.Name) ? floor.Name : dto.Name);
    }

    [Fact]
    public void Update_throw_exception_when_floor_not_found()
    {
        var floorId = -1;
        var dto = new PutFloorDto()
        {
            Id = floorId,
            Name = "dummy"
        };

        var actual = () => _sut.Update(dto);

        actual.Should().ThrowExactly<FloorNotFoundException>();
    }

    [Fact]
    public void Update_throw_exception_when_name_duplicated()
    {
        var block = new Block()
        {
            Name = "CreateFloorWithUnits",
            CreationDate = DateTime.UtcNow,
            FloorCount = 2
        };
        Save(block);
        var floor = new Floor()
        {
            Name = "CreateFloorWithUnits",
            UnitCount = 4,
            BlockId = block.Id
        };
        Save(floor);
        var floor2 = new Floor()
        {
            Name = "name2",
            UnitCount = 8,
            BlockId = block.Id
        };
        Save(floor2);
        var dto = new PutFloorDto()
        {
            Id = floor.Id,
            Name = "name2"
        };

        var actual = () => _sut.Update(dto);

        actual.Should().ThrowExactly<FloorNameDuplicateInBlockException>();
    }
}