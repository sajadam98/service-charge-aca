using System.ComponentModel;
using FluentAssertions;
using Moq;
using ServiceCharge.Persistence.Ef.Units;
using ServiceCharge.Services.Floors.Exceptions;
using ServiceCharge.Services.Units.Contracts;
using ServiceCharge.Services.Units.Exceptions;
using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;
using ServiceCharge.TestTools.Floors.DTOs;
using ServiceCharge.TestTools.Units;
using ServiceCharge.TestTools.Units.DTOs;

namespace ServiceCharge.Service.Unit.Tests.Units;

public class UnitServiceTests:BusinessIntegrationTest
{
    private readonly UnitService _sut;
    
     public UnitServiceTests()
     {
        // _dateTimeServiceMock = new Mock<DateTimeService>();
        // _dateTimeServiceMock.Setup(_ => _.NowUtc)
        //     .Returns(new DateTime(2020, 1, 1));
       
        _sut = UnitServiceFactory.CreateUnitService(SetupContext,
            new EFUnitRepository(SetupContext));
    }

    [Fact]
    public void Add_unit_properly()
    {
        var blockBuilder = new BlockBuilder();
        var block = blockBuilder.WithFloorCount(2).Build();
        Save(block);
        var floor = FloorFactory.Create(block.Id);
        Save(floor);
        var unitDto = UnitDtoFactory.CreateAddUnitDto("name", floor.Id);

        _sut.Add(unitDto);

        var actual = ReadContext.Set<Entities.Unit>().ToList();
        actual.Should().HaveCount(1);
        actual.Should().Contain(_ => _.Name == unitDto.Name && _.FloorId == unitDto.FloorId);
    }

    [Theory]
    [InlineData(-1)]
    public void Add_throw_exception_when_floor_not_found(int invalidFloorId)
    {
       
        var unitDto = UnitDtoFactory.CreateAddUnitDto("name", invalidFloorId);

        var actual = () => _sut.Add(unitDto);

        actual.Should().ThrowExactly<FloorNotFoundException>();
    }

    [Fact]
    public void Add_throw_exception_when_floor_unit_count_is_full()
    {
        var blockBuilder = new BlockBuilder();
        var block = blockBuilder.WithFloorCount(2).Build();
        Save(block);
        var floor = FloorFactory.Create(block.Id, unitCount:1);
        Save(floor);
        var unit = UnitFactory.Create(floor.Id);
        Save(unit);
        var unitDto = UnitDtoFactory.CreateAddUnitDto("name",floor.Id);
        
        
        var actual = () => _sut.Add(unitDto);
    
        actual.Should().ThrowExactly<MaxUnitCountException>();
    }
    
    [Fact]
    public void Delete_delete_a_unit_properly()
    {
        var blockBuilder1 = new BlockBuilder();
        var block1 = blockBuilder1.WithFloorCount(2).WithName("Block1").Build();
        Save(block1);
        var floor1 = FloorFactory.Create(block1.Id, "Floor1");
        Save(floor1);
        var unit1 = UnitFactory.Create(floor1.Id, "Unit1");
        Save(unit1);
        var unit2 = UnitFactory.Create(floor1.Id, "Unit2");
        Save(unit2);
    
        _sut.Delete(unit1.Id);
    
        var actual = ReadContext.Set<Entities.Unit>().Single();
        var actual2 = ReadContext.Set<Entities.Unit>().Any(_ => _.Id == unit1.Id);
        actual2.Should().BeFalse();
    }
    
    [Theory]
    [InlineData(-1)]
    public void Delete_throws_exception_if_unit_does_not_exist(int invalidUnitId)
    {
    
        var actual = () => _sut.Delete(invalidUnitId);
    
        actual.Should().ThrowExactly<UnitNotFoundException>();
    }

    [Fact]
    public void Update_update_a_unit_properly()
    {
        var blockBuilder1 = new BlockBuilder();
        var block1 = blockBuilder1.WithFloorCount(2).WithName("Block1").Build();
        Save(block1);
        var floor1 = FloorFactory.Create(block1.Id, "Floor1");
        Save(floor1);
        var unit1 = UnitFactory.Create(floor1.Id, "Unit1");
        Save(unit1);
        var updateDto = UnitDtoFactory.CreateUpdateUnitDto("name", false );

        _sut.Update(unit1.Id, updateDto);
        
        ReadContext.Set<Entities.Unit>().Should().HaveCount(1)
            .And.OnlyContain(u => u.Id == unit1.Id 
                                  &&u.Name == updateDto.Name 
                                  && u.FloorId == floor1.Id 
                                  && u.IsActive == updateDto.IsActive);
    }

    [Theory]
    [InlineData(-1)]
    public void Update_throw_exception_when_unit_does_not_exist(int invalidUnitId)
    {
        var dto = UnitDtoFactory.CreateUpdateUnitDto("name", false );
        var actual = () => _sut.Update(invalidUnitId, dto);

        actual.Should().ThrowExactly<UnitNotFoundException>();
    }

    [Theory]
    [InlineData("jamali")]
    public void Update_throw_exception_when_unit_name_is_duplicated(string duplicateName)
    {
        var blockBuilder1 = new BlockBuilder();
        var block = blockBuilder1.WithFloorCount(2).WithName("Block1").Build();
        Save(block);
        var floor = FloorFactory.Create(block.Id, "Floor1");
        Save(floor);
        var unit1 = UnitFactory.Create(floor.Id, duplicateName);
        Save(unit1);
        var unit2 = UnitFactory.Create(floor.Id);
        Save(unit2);
        var dto = UnitDtoFactory.CreateUpdateUnitDto(duplicateName);
        
        var actual = () => _sut.Update(unit2.Id, dto);

        actual.Should().ThrowExactly<DuplicateUnitNameException>();
        ReadContext.Set<Entities.Unit>().Any(u => u.Id == unit2.Id 
                                                  && u.Name == dto.Name)
                                                    .Should().BeFalse();
    }
    
}