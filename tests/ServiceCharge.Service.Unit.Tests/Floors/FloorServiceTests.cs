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
using ServiceCharge.TestTools.Floors.DTOs;

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
        var dto = FloorDtoFactory.AddFloorDto(3);
        
        
        var actual = _sut.Add(block.Id, dto);

        var expected =
            ReadContext.Set<Floor>().Single(f => f.Id == actual);
        expected.Name.Should().Be(dto.Name);
        expected.UnitCount.Should().Be(dto.UnitCount);
        expected.BlockId.Should().Be(block.Id);
    }

    [Theory]
    [InlineData(-1)]
    public void Add_throw_exception_when_block_not_exist_exception(int invalidBlockId)
    {
        var dto = FloorDtoFactory.AddFloorDto(1);

        var actual = () => _sut.Add(invalidBlockId, dto);

        actual.Should().ThrowExactly<BlockNotFoundException>();
        ReadContext.Set<Floor>().Should().BeEmpty();
    }

    [Theory]
    [InlineData("Yas")]
    public void
        Add_throw_exception_when_floor_does_exist_in_same_block_with_given_name(string name)
    {
        var blockBuilder = new BlockBuilder();
        var block = blockBuilder.WithFloorCount(2).Build();
        Save(block);
        var floor = FloorFactory.Create( block.Id, name, 3);
        Save(floor);
        var dto = FloorDtoFactory.AddFloorDto(3, name);
           

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
    public void Add_adds_floor_with_same_name_in_different_blocks_properly(string floorName)
    {
        var blockBuilder1 = new BlockBuilder();
        var block1 = blockBuilder1.WithFloorCount(2).WithName("Block1").Build();
        Save(block1);
        var blockBuilder2 = new BlockBuilder();
        var block2 = blockBuilder2.WithFloorCount(2).WithName("Block2").Build();
        Save(block2);
        var floor = FloorFactory.Create( block1.Id, floorName, 1);
        Save(floor);
        var dto = FloorDtoFactory.AddFloorDto(1, floorName);
           
        
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
        var blockBuilder = new BlockBuilder();
        var block = blockBuilder.WithFloorCount(1).Build();
        Save(block);
        var floor = FloorFactory.Create( block.Id, unitCount:2);
        Save(floor);
        var dto = FloorDtoFactory.AddFloorDto(1, "floor2");

        var actual = () => _sut.Add(block.Id, dto);

        actual.Should().ThrowExactly<BlockFloorsCapacityFulledException>();
        ReadContext.Set<Floor>().Should().HaveCount(1)
            .And.ContainSingle(f =>
                f.Id == floor.Id && f.BlockId == floor.BlockId);
    }

    [Fact]
    public void Update_update_floor_properly()
    {
        var blockBuilder = new BlockBuilder();
        var block = blockBuilder.WithFloorCount(2).Build();
        Save(block);
        var floor = FloorFactory.Create( block.Id, unitCount:2);
        Save(floor);
        var dto = FloorDtoFactory.UpdateFloorDto("floor1", 2);

        _sut.Update(floor.Id, dto);

        ReadContext.Set<Floor>().Single(_ => _.Id == floor.Id).Should().NotBeNull()
            .And.BeEquivalentTo(dto);
    }


    [Theory]
    [InlineData(-1)]
    public void Update_throw_exception_when_floor_not_exist_exception(int invalidFloorId)
    {
        var dto = FloorDtoFactory.UpdateFloorDto("name", 1);

        var actual = () => _sut.Update(invalidFloorId, dto);

        actual.Should().ThrowExactly<FloorNotFoundException>();
        
    }

    [Theory]
    [InlineData("floor1")]
    public void Update_throw_exception_when_floor_name_is_duplicated(string invalidFloorName)
    {
        var blockBuilder = new BlockBuilder();
        var block = blockBuilder.WithFloorCount(2).Build();
        Save(block);
        var floor = FloorFactory.Create( block.Id,invalidFloorName,  2);
        Save(floor);
        var floor2 = FloorFactory.Create(block.Id, unitCount: 2);
        Save(floor2);
        var dto = FloorDtoFactory.UpdateFloorDto(invalidFloorName, 1);
        
        var actual = () => _sut.Update(floor2.Id, dto);

        actual.Should().ThrowExactly<DuplicateFloorNameException>();
        ReadContext.Set<Floor>().Single(_ => _.Id == floor2.Id).Name.Should().Be(invalidFloorName);
    }

    [Fact]
    public void Delete_delete_floor_properly()
    {
        var blockBuilder = new BlockBuilder();
        var block = blockBuilder.WithFloorCount(2).Build();
        Save(block);
        var floor = FloorFactory.Create( block.Id, unitCount: 2);
        Save(floor);
        
        _sut.Delete(floor.Id);

        ReadContext.Set<Floor>().Should().HaveCount(0);
    }

    [Theory]
    [InlineData(-1)]
    public void Delete_throw_exception_when_floor_not_exist_exception(int invalidFloorId)
    {
        
        var actual = () => _sut.Delete(invalidFloorId);

        actual.Should().ThrowExactly<FloorNotFoundException>();
    }
}