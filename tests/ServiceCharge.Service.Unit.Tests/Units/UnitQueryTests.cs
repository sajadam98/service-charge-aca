﻿using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;
using ServiceCharge.TestTools.Infrastructure.DataBaseConfig.Integration;
using ServiceCharge.TestTools.Units;

namespace ServiceCharge.Service.Unit.Tests.Units;

public class UnitQueryTests : BusinessIntegrationTest
{
    private readonly UnitQuery _sut;

    public UnitQueryTests()
    {
        _sut = new EFUnitQuery(ReadContext);
    }
    
    [Fact]
    public void GetById_get_a_unit_properly()
    {
        var block1 = BlockFactory.Create("Block1",2);
        var floor1 = FloorFactory.Generate(block1.Id,"Floor1",unitCount:5);
        block1.Floors.Add(floor1);
        var floor2 = FloorFactory.Generate(block1.Id,"Floor2",unitCount:7);
        block1.Floors.Add(floor2);
        Save(block1);
        var unit1 = UnitFactory.Create(floor1.Id);
        Save(unit1);
        var unit2 = UnitFactory.Create(floor1.Id);
        Save(unit2);
        var block2 = BlockFactory.Create("Block2",5);
        Save(block2);

        var result = _sut.GetById(unit1.Id);
        
        result.Name.Should().Be(unit1.Name);
        result.FloorId.Should().Be(floor1.Id);
        result.IsActive.Should().Be(unit1.IsActive);
    }
    [Fact]
    public void
        GetAllWithBlockNameAndFloorName_get_all_units_with_block_name_and_floor_name()
    {
        var block1 = BlockFactory.Create("Block1");
        Save(block1);
        var floor1 = FloorFactory.Generate(block1.Id,"Floor1",block1.Id);
        Save(floor1);
        var unit1 = UnitFactory.Create(floor1.Id);
        Save(unit1);
        var unit2 = UnitFactory.Create( floor1.Id);
        Save(unit2);
        var floor2 = FloorFactory.Generate(block1.Id,"Floor2",block1.Id);
        Save(floor2);
        var unit3 = UnitFactory.Create( floor2.Id);
        Save(unit3);
        var unit4 = UnitFactory.Create(floor2.Id);
        Save(unit4);

        var result = _sut.GetAllWithBlockNameAndFloorName();

        result.Should().HaveCount(4);

        
        result.Should().ContainEquivalentOf(
            UnitFactory.GetAllUnitsWithInfo
                (unit1.Id,unit1.Name,floor1.Id,unit1.IsActive,floor1.Name,block1.Name));
        
        result.Should().ContainEquivalentOf(
            UnitFactory.GetAllUnitsWithInfo
                (unit2.Id,unit2.Name,floor1.Id,unit2.IsActive,floor1.Name,block1.Name));
        
        result.Should().ContainEquivalentOf(
            UnitFactory.GetAllUnitsWithInfo
                (unit3.Id,unit3.Name,floor2.Id,unit3.IsActive,floor2.Name,block1.Name));
        
        result.Should().ContainEquivalentOf(
            UnitFactory.GetAllUnitsWithInfo
                (unit4.Id,unit4.Name,floor2.Id,unit4.IsActive,floor2.Name,block1.Name));
    }
}