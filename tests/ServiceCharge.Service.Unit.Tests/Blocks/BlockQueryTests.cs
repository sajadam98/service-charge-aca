using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;
using ServiceCharge.TestTools.Infrastructure.DataBaseConfig.Integration;

namespace ServiceCharge.Service.Unit.Tests.Blocks;

public class BlockQueryTests : BusinessIntegrationTest
{
    private readonly BlockQuery _sut;

    public BlockQueryTests()
    {
        _sut = new EFBlockQuery(ReadContext);
    }

    [Fact]
    public void GetById_get_a_block_properly()
    {
        var block1 = BlockFactory.Create("Block1", 2);
        var floor1 =
            FloorFactory.Generate(block1.Id, "Floor1", unitCount: 5);
        block1.Floors.Add(floor1);
        var floor2 =
            FloorFactory.Generate(block1.Id, "Floor2", unitCount: 7);
        block1.Floors.Add(floor2);
        Save(block1);
        var block2 = BlockFactory.Create("Block2", 5);
        Save(block2);

        var result = _sut.GetById(block1.Id);

        result.Should().NotBeNull();
        result.Name.Should().Be(block1.Name);
        result.FloorCount.Should().Be(block1.FloorCount);
        result.CreationDate.Should().Be(block1.CreationDate);
    }

    [Fact]
    public void GetAll_get_all_blocks_properly()
    {
        var block1 = BlockFactory.Create("Block1");
        Save(block1);
        var block2 = BlockFactory.Create("Block2");
        Save(block2);

        var actual = _sut.GetAll();


        actual.Should().ContainEquivalentOf(
            BlockFactory.GetAll
            (block1.Id, block1.Name, block1.FloorCount,
                block1.CreationDate));
        actual.Should().ContainEquivalentOf(
            BlockFactory.GetAll
            (block2.Id, block2.Name, block2.FloorCount,
                block2.CreationDate));
    }

    [Fact]
    public void GetByIdAndFloorsInfo_get_by_id_and_floors_info_properly()
    {
        var block1 = BlockFactory.Create("Block1");
        var floor1 = FloorFactory.Generate(block1.Id, "Floor1");
        block1.Floors.Add(floor1);
        var floor2 = FloorFactory.Generate(block1.Id, "Floor2");
        block1.Floors.Add(floor2);
        Save(block1);
        var block2 = BlockFactory.Create("Block2");
        Save(block2);

        var actual = _sut.GetByIdAndFloorsInfo(block1.Id);

        actual.Name.Should().Be(block1.Name);
        actual.FloorCount.Should().Be(block1.FloorCount);
        actual.CreationDate.Should().Be(block1.CreationDate);
        actual.Floors.Should().ContainEquivalentOf(
            FloorFactory.GetAll(floor1.Id, floor1.BlockId, floor1.Name,
                floor1.UnitCount)
            , _ => _.Excluding(_ => _.Id));
    }

    [Fact]
    public void GetAllWithFloorsInfo_gets_all_blocks_with_floors_properly()
    {
        var block1 = BlockFactory.Create("Block1");
        Save(block1);
        var floor1 = FloorFactory.Generate(block1.Id,"Floor1", block1.Id);
        Save(floor1);
        var floor2 = FloorFactory.Generate(block1.Id,"Floor2", block1.Id);
        Save(floor2);
        var block2 = BlockFactory.Create("Block2", 4);
        Save(block2);

        var result = _sut.GetAllWithFloorsInfo();

        result.Should().HaveCount(2);

        var block1Dto = new GetAllBlocksWithFloorsDtoBuilder()
            .WithName(block1.Name)
            .WithFloorCount(block1.FloorCount)
            .WithCreationDate(block1.CreationDate)
            .WithId(block1.Id)
            .AddFloor(new GetAllFloorsDtoBuilder()
                .WithName(floor1.Name)
                .WithUnitCount(floor1.UnitCount)
                .WithId(floor1.Id)
                .WithBlockId(floor1.BlockId)
                .Build())
            .AddFloor(new GetAllFloorsDtoBuilder()
                .WithName(floor2.Name)
                .WithUnitCount(floor2.UnitCount)
                .WithId(floor2.Id)
                .WithBlockId(floor2.BlockId)
                .Build())
            .Build();

        result.Should().ContainEquivalentOf(block1Dto);

        var block2Dto = new GetAllBlocksWithFloorsDtoBuilder()
            .WithName(block2.Name)
            .WithFloorCount(block2.FloorCount)
            .WithCreationDate(block2.CreationDate)
            .WithId(block2.Id)
            .Build();

        result.Should().ContainEquivalentOf(block2Dto);
    }
}