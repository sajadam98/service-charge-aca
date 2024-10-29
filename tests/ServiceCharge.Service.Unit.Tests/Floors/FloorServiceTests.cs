using FluentAssertions;
using ServiceCharge.Entities;
using ServiceCharge.Services.Blocks.Exceptions;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.Floors.Contracts.Dto;
using ServiceCharge.Services.Floors.Exceptions;
using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;

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
        Add_throw_exception_when_floor_does_exist_in_same_block_with_given_name(
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
    [InlineData("Yas")]
    public void Add_adds_floor_with_same_name_in_different_blocks_properly(
        string floorName)
    {
        var block1 = new Block
        {
            Name = "Dummy_Block_Name",
            CreationDate = DateTime.Now,
            FloorCount = 2
        };
        Save(block1);
        var block2 = new Block
        {
            Name = "Dummy_Block_Name",
            CreationDate = DateTime.Now,
            FloorCount = 2
        };
        Save(block2);
        var floor = new Floor
        {
            Name = floorName,
            UnitCount = 1,
            BlockId = block1.Id
        };
        Save(floor);
        var dto = new AddFloorDto
        {
            Name = floorName,
            UnitCount = 1,
        };

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
        var block = new Block
        {
            Name = "Dummy_Block_Name",
            CreationDate = DateTime.Now,
            FloorCount = 1,
        };
        Save(block);
        var floor = new Floor
        {
            Name = "Dummy_Floor_Name",
            UnitCount = 1,
            BlockId = block.Id
        };
        Save(floor);
        var dto = new AddFloorDto
        {
            Name = "Dummy_Floor_Name_2",
            UnitCount = 1
        };

        var actual = () => _sut.Add(block.Id, dto);

        actual.Should().ThrowExactly<BlockFloorsCapacityFulledException>();
        ReadContext.Set<Floor>().Should().HaveCount(1)
            .And.ContainSingle(f =>
                f.Id == floor.Id && f.BlockId == floor.BlockId);
    }

    [Fact]
    public void Update_Floor_properly()
    {
        var floors = new HashSet<Floor>()
        {
            new Floor()
            {
                Name = "1",
                UnitCount = 3
            },
            {
                new Floor()
                {
                    Name = "2",
                    UnitCount = 3
                }
            }
        };
        var block = CreateBlock("name", DateTime.UtcNow, 2, floors);
        Save(block);

        var updateDto = new UpdateFloorDto()
        {
            Name = "name2"
        };

        var floorId = block.Floors.Select(_ => _.Id).First();


        _sut.Update(updateDto, floorId);

        var actual = ReadContext.Set<Floor>().Find(floorId);

        actual!.Name.Should().Be(updateDto.Name);
    }

    [Fact]
    public void Update_throw_exception_when_floor_not_found()
    {
        var floorId = -1;

        var updateDto = new UpdateFloorDto()
        {
            Name = "name2"
        };

        var actual = () => _sut.Update(updateDto, floorId);

        actual.Should().ThrowExactly<FloorNotFoundException>();
    }

    [Fact]
    public void Update_throw_exception_when_floor_name_is_duplicate()
    {
        var floors = new HashSet<Floor>()
        {
            new Floor()
            {
                Name = "1",
                UnitCount = 3
            },
            {
              new Floor()
                {
                    Name = "2",
                    UnitCount = 3
                }
            }
        };
        var block = CreateBlock("name", DateTime.UtcNow, 2, floors);
        Save(block);

        var updateDto = new UpdateFloorDto()
        {
            Name = "1"
        };

        var floorId = block.Floors.Select(_ => _.Id).LastOrDefault();
        var actual = () => _sut.Update(updateDto, floorId);

        actual.Should().ThrowExactly<DuplicateFloorNameException>();
    }



    private static AddFloorDto CreateAddFloorDto(int blockId, string name, int unitCount)
    {
        var dto = new AddFloorDto()
        {
            Name = name,
            UnitCount = unitCount
        };
        return dto;
    }

    private static Block CreateBlock(string name, DateTime dateTime, int flourCount, HashSet<Floor> floors)
    {
        return new Block()
        {
            Name = name,
            CreationDate = dateTime,
            FloorCount = flourCount,
            Floors = floors
        };
    }


}