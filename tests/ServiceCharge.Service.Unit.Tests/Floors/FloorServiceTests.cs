using FluentAssertions;
using ServiceCharge.Entities;
using ServiceCharge.Persistence.Ef.Floors;
using ServiceCharge.Persistence.Ef.UnitOfWorks;
using ServiceCharge.Services.Blocks.Exceptions;
using ServiceCharge.Services.Floors;
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

    [Theory]
    [InlineData("Dumy", "yas")]
    public void Update_Update_floor_properly(string floorName,string updateFloorName)
    {
        var block=new BlockBuilder().WithFloorCount(2).Build();
        Save(block);
        var floor = FloorFactory.Create(block.Id, floorName);
        Save(floor);
        
        _sut.Update(block.Id, floor.Name, updateFloorName);
        
        var expected = ReadContext.Set<Floor>().Single(f => f.Id == floor.Id);
        
        expected.Name.Should().Be(updateFloorName);
    }
    
    [Theory]
    [InlineData("Dumy", "yas")]
    public void Update_Throw_Exception_when_block_isent_exist(string floorName,string updateFloorName)
    {
        var block=new BlockBuilder().WithFloorCount(2).Build();
        
        var floor = FloorFactory.Create(block.Id, floorName);
        
        
        var actual=()=>_sut.Update(block.Id, floor.Name, updateFloorName);
        
        actual.Should().ThrowExactly<BlockNotFoundException>();
    }
    
    [Theory]
    [InlineData("Dumy", "yas")]
    public void Update_Throw_Exception_when_floor_isent_exist(string floorName,string updateFloorName)
    {
        var block=new BlockBuilder().WithFloorCount(2).Build();
        Save(block);
        var floor = FloorFactory.Create(block.Id, floorName);
        
        
        var actual=()=>_sut.Update(block.Id, floor.Name, updateFloorName);
        
        actual.Should().ThrowExactly<FloorIsentExistException>();
    }
    
    [Theory]
    [InlineData("Dumy", "Dumy")]
    public void Update_Throw_Exception_when_floor_dublicate_name(string floorName,string updateFloorName)
    {
        var block=new BlockBuilder().WithFloorCount(2).Build();
        Save(block);
        var floor = FloorFactory.Create(block.Id, floorName);
        Save(floor);
        
        var actual=()=>_sut.Update(block.Id, floor.Name, updateFloorName);
        
        actual.Should().ThrowExactly<DuplicateFloorNameException>();
    }

    [Fact]
    public void Delete_delete_floor_properly()
    {
        var block=new BlockBuilder().WithFloorCount(2).Build();
        Save(block);
        var floor = FloorFactory.Create(block.Id, "Dumy");
        Save(floor);
        var floor2 = FloorFactory.Create(block.Id, "yyy");
        Save(floor2);
        _sut.Delete(block.Id,floor.Name);

        var actual = ReadContext.Set<Floor>().SingleOrDefault(_ => _.Id == floor.Id);
        
        actual.Should().BeNull();

    }

    [Fact]
    public void Delete_throw_exception_when_block_isent_exist()
    {
        var block=new BlockBuilder().WithFloorCount(2).Build();
        
        var floor = FloorFactory.Create(block.Id, "Dumy");
        
        
        var actual=()=>_sut.Delete(block.Id,floor.Name);

        actual.Should().ThrowExactly<BlockNotFoundException>();
        
    }
    
    [Fact]
    public void Delete_throw_exception_when_floor_isent_exist()
    {
        var block=new BlockBuilder().WithFloorCount(2).Build();
        Save(block);
        var floor = FloorFactory.Create(block.Id, "Dumy");
        
        
        var actual=()=>_sut.Delete(block.Id,floor.Name);

        actual.Should().ThrowExactly<FloorIsentExistException>();
        
    }
    
    
}