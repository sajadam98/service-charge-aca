using ServiceCharge.Persistence.Ef;
using ServiceCharge.Services.Floors.Contracts.Dtos;

namespace ServiceCharge.Service.Unit.Tests.Blocks;

public class BlockQueryTests : BusinessIntegrationTest
{
    private readonly BlockQuery _sut;

    public BlockQueryTests()
    {
        _sut = new EfBlockQuery(Context);
    }

    [Fact]
    public void GetAll_get_all_blocks_properly()
    {
        var block = new Block()
        {
            Name = "name1",
            CreationDate = new DateTime(2024, 11, 12),
            FloorCount = 4
        };
        Save(block);
        var block2 = new Block()
        {
            Name = "name2",
            CreationDate = new DateTime(2024, 10, 9),
            FloorCount = 8,
            Floors =
            [
                new()
                {
                    Name = "floor1",
                    UnitCount = 2
                }
            ]
        };
        Save(block2);

        HashSet<GetBlockDto> result = _sut.GetAll();
        var actual = ReadContext.Set<Block>()
            .Select(_ =>
                new GetBlockDto()
                {
                    Id = _.Id,
                    Name = _.Name,
                    CreationDate = _.CreationDate,
                    FloorCount = _.FloorCount,
                }).ToHashSet();
        actual.Should().BeEquivalentTo(result);
        actual.Should().HaveCount(2);
        actual.Should().Contain(_ => _.Id == block.Id && _.Name == block.Name);
        actual.Should()
            .Contain(_ => _.Id == block2.Id && _.Name == block2.Name);
    }

    [Fact]
    public void GetAllWithFloors_get_all_blocs_with_floors_properly()
    {
        var block1 = new Block()
        {
            Name = "name1",
            CreationDate = new DateTime(2024, 10, 9),
            FloorCount = 4,
            Floors =
            [
                new()
                {
                    Name = "floor1",
                    UnitCount = 3
                }
            ]
        };
        Save(block1);
        var block2 = new Block()
        {
            Name = "name2",
            CreationDate = new DateTime(2024, 10, 9),
            FloorCount = 8,
            Floors =
            [
                new()
                {
                    Name = "floor1",
                    UnitCount = 2
                },
                new()
                {
                    Name = "floor2",
                    UnitCount = 2
                }
            ]
        };
        Save(block2);

        HashSet<GetBlockWithFloorsDto> result = _sut.GetAllWithFloors();
        // var actual = ReadContext.Set<Block>()
        //     .Include(_ => _.Floors)
        //     .ToHashSet();
        result.Should().HaveCount(2);
        result.Single(_ => _.Id == block1.Id).Floors
            .Should()
            .HaveCount(block1.Floors.Count);
        result.Single(_ => _.Id == block2.Id).Floors
            .Should()
            .HaveCount(block2.Floors.Count);
        result.First().Floors.First().Should().BeEquivalentTo(new GetFloorDto()
        {
            Id = block1.Floors.First().Id,
            Name = block1.Floors.First().Name,
            UnitCount = block1.Floors.First().UnitCount,
            BlockId = block1.Floors.First().BlockId,
        });
    }
}