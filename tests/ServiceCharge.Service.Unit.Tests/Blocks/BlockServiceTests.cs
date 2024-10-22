using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using ServiceCharge.Entities;
using ServiceCharge.Persistence.Ef.UnitOfWorks;
using ServiceCharge.Services;
using ServiceCharge.Services.Blocks.Exceptions;

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
            Name = "name",
            CreationDate = DateTime.UtcNow
        };
        Save(block);
        var dto = new AddBlockDto()
        {
            Name = "name"
        };

        var actual = () => _sut.Add(dto);

        actual.Should().ThrowExactly<BlockNameDuplicateException>();
    }

    [Fact]
    public void AddWithFloor_()
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
        actual.Floors.Should().Contain(_ => _.Name == "floor1" && _.UnitCount == 2);
    }
}