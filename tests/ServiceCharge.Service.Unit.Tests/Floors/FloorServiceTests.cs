using System.Runtime.InteropServices;
using FluentAssertions;
using ServiceCharge.Entities;
using ServiceCharge.Persistence.Ef.Floors;
using ServiceCharge.Persistence.Ef.UnitOfWorks;
using ServiceCharge.Services.Blocks.Exceptions;
using ServiceCharge.Services.Floors;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.Floors.Contracts.Dto;
using ServiceCharge.Services.Floors.Exceptions;

namespace ServiceCharge.Service.Unit.Tests.Floors;

public class FloorServiceTests : BusinessIntegrationTest
{
    private readonly FloorService _sut;

    public FloorServiceTests()
    {
        var repository = new EFFloorRepository(SetupContext);
        var blockRepository = new EFBlockRepository(SetupContext);
        var unitOfWork = new EfUnitOfWork(SetupContext);
        _sut = new FloorAppService(repository, blockRepository,
            unitOfWork);
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
    public void Add_throw_exception_when_block_not_exist_exception(int invalidBlockId)
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
    public void Add_throw_exception_when_floor_does_exist_in_same_block_with_given_name(string name)
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
    
    [Fact]
    public void Add_Throw_Exception_When_Block_Is_Full()
    {
        var block = new Block
        {
            Name = "dummy_block",
            CreationDate = DateTime.Now,
            FloorCount = 1,
            
        };
        block.Floors = new HashSet<Floor>()
        {
            new()
            {
                Name = "1",
                UnitCount = 4,
            }
        };
        Save(block);
        var dto = new AddFloorDto()
        {
            Name = "floor2",
            UnitCount = 4
        };
        
        var actual = () => _sut.Add(block.Id,dto);

        
        actual.Should().ThrowExactly<MaxCountFloorException>();
        ReadContext.Set<Floor>().Should().HaveCount(1);
    }


    [Fact]
    public void Add_Floor_to_two_different_block_and_not_throw_exception()
    {
        var block = new Block
        {
            Name = "dummy_block",
            CreationDate = DateTime.Now,
            FloorCount = 1,
            
        };
        Save(block);
        var block1 = new Block
        {
            Name = "dummy_block2",
        };
        Save(block1);
        
    }
}