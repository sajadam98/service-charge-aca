using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;
using ServiceCharge.TestTools.Infrastructure.DataBaseConfig.Integration;
using ServiceCharge.TestTools.Units;

namespace ServiceCharge.Service.Unit.Tests.Floors;

public class FloorQueryTests : BusinessIntegrationTest
{
    private readonly FloorQuery _sut;

    public FloorQueryTests()
    {
        _sut = new EFFloorQuery(ReadContext);
    }
    [Fact]
    public void GetById_get_a_floor_properly()
    {
        var block1 = BlockFactory.Create("Block1",2);
        var floor1 = FloorFactory.Generate(block1.Id,"Floor1",unitCount:5);
        block1.Floors.Add(floor1);
        var floor2 = FloorFactory.Generate(block1.Id,"Floor2",unitCount:7);
        block1.Floors.Add(floor2);
        Save(block1);
        var block2 = BlockFactory.Create("Block2",5);
        Save(block2);

        var result = _sut.GetById(floor1.Id);
        
        result.Name.Should().Be(floor1.Name);
        result.UnitCount.Should().Be(floor1.UnitCount);
        result.BlockId.Should().Be(floor1.BlockId);
        result.Id.Should().Be(floor1.Id);
    }
    
    [Fact]
    public void GetAll_get_all_floors_properly()
    {
        var block1 = BlockFactory.Create("block1");
        Save(block1);
        var floor1 = FloorFactory.Generate(block1.Id,"floor1",block1.Id);
        Save(floor1);
        var floor2 = FloorFactory.Generate(block1.Id,"floor2",block1.Id);
        Save(floor2);

        var result = _sut.GetAll();

        result.Should().HaveCount(2);
        result.Should().ContainEquivalentOf(FloorFactory
            .GetAll(floor1.Id,floor1.BlockId,floor1.Name,floor1.UnitCount));
        result.Should().ContainEquivalentOf(FloorFactory
            .GetAll(floor2.Id,floor2.BlockId,floor2.Name,floor2.UnitCount));
    }

    [Fact]
    public void GetAllWithUnits_get_all_floors_with_units_properly()
    {
        var block1 = BlockFactory.Create("block1");
        Save(block1);
        var floor1 = FloorFactory.Generate(block1.Id,"floor1",block1.Id);
        Save(floor1);
        var unit1 = UnitFactory.Create("unit1",floor1.Id);
        Save(unit1);
        var unit2 = UnitFactory.Create("unit2",floor1.Id);
        Save(unit2);
        var floor2 = FloorFactory.Generate(block1.Id,"floor2",block1.Id);
        Save(floor2);
        var unit3 = UnitFactory.Create("unit3",floor2.Id);
        Save(unit3);
        var unit4 = UnitFactory.Create("unit4",floor2.Id);
        Save(unit4);

        var result = _sut.GetAllFloorsWithUnits();
        
        var floor1Dto = new GetAllFloorsWithUnitsBuilder()
            .WithName(floor1.Name)
            .WithUnitCount(floor1.UnitCount)
            .WithBlockId(floor1.BlockId)
            .WithId(floor1.Id)
            .AddUnit(new GetAllUnitsBuilder()
                .WithName(unit1.Name)
                .WithFloorId(floor1.Id)
                .WithIsActive(unit1.IsActive)
                .WithId(unit1.Id)
                .Build())
            .AddUnit(new GetAllUnitsBuilder()
                .WithName(unit2.Name)
                .WithFloorId(floor1.Id)
                .WithIsActive(unit2.IsActive)
                .WithId(unit2.Id)
                .Build())
            .Build();
        var floor2Dto = new GetAllFloorsWithUnitsBuilder()
            .WithName(floor2.Name)
            .WithUnitCount(floor2.UnitCount)
            .WithBlockId(floor2.BlockId)
            .WithId(floor2.Id)
            .AddUnit(new GetAllUnitsBuilder()
                .WithName(unit3.Name)
                .WithFloorId(floor2.Id)
                .WithIsActive(unit3.IsActive)
                .WithId(unit3.Id)
                .Build())
            .AddUnit(new GetAllUnitsBuilder()
                .WithName(unit4.Name)
                .WithFloorId(floor2.Id)
                .WithIsActive(unit4.IsActive)
                .WithId(unit4.Id)
                .Build())
            .Build();
        result.Should().ContainEquivalentOf(floor1Dto);
        result.Should().ContainEquivalentOf(floor2Dto);
        result.Should().HaveCount(2);

    }
}