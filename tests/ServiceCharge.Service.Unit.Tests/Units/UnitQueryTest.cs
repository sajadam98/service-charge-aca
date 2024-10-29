using System.Net;
using ServiceCharge.Persistence.Ef.Units;
using ServiceCharge.Service.Unit.Tests.Blocks;
using ServiceCharge.Service.Unit.Tests.Floors;
using ServiceCharge.Services.Units.Contracts;

namespace ServiceCharge.Service.Unit.Tests.Units;

public class UnitQueryTest : BusinessIntegrationTest
{
    private readonly UnitQuery _sut;

    public UnitQueryTest()
    {
        _sut = new EfUnitQuery(Context);
    }

    [Fact]
    public void
        GetAllUnitsWithFloorAndBlockName_get_all_units_with_floor_name_and_block_name_properly()
    {
        var block1 = BlockFactory.Create(name: "block_1");
        Save(block1);
        var floor1 = FloorFactory.Crerate(blockId: block1.Id, name: "Floor_1");
        Save(floor1);
        var floor2 = FloorFactory.Crerate(blockId: block1.Id, name: "floor_2");
        Save(floor2);
        var unit1 = UnitFactory.Create(floorId: floor1.Id, name: "unit_11");
        Save(unit1);
        var unit2 = UnitFactory.Create(floorId: floor2.Id, name: "unit_21");
        Save(unit2);
        var unit3 = UnitFactory.Create(floorId: floor2.Id, name: "unit_22");
        Save(unit3);
        var unit4 = UnitFactory.Create(floorId: floor2.Id, name: "unit_23");
        Save(unit4);

        var result = _sut.GetAllUnitsWithFloorAndBlockName();

        var expected =
            (from unit in ReadContext.Set<Entities.Unit>()
                join floor in ReadContext.Set<Floor>()
                    on unit.FloorId equals floor.Id
                join block in ReadContext.Set<Block>()
                    on floor.BlockId equals block.Id
                select new GetAllUnitsWithFloorAndBlockNameDto()
                {
                    Id = unit.Id,
                    Name = unit.Name,
                    FloorName = floor.Name,
                    BlockName = block.Name,
                    FloorId = floor.Id,
                    IsActive = unit.IsActive
                });

        expected.Should().BeEquivalentTo(expected);
    }
}