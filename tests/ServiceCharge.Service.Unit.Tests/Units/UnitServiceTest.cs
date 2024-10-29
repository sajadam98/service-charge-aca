using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using ServiceCharge.Entities;
using ServiceCharge.Persistence.Ef;
using ServiceCharge.Persistence.Ef.Floors;
using ServiceCharge.Persistence.Ef.UnitOfWorks;
using ServiceCharge.Services.Floors;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.UnitOfWorks;
using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;
using ServiceCharge.TestTools.Units;

namespace ServiceCharge.Service.Unit.Tests.Units;

public class UnitServiceTest:BusinessIntegrationTest
{
    private readonly UnitService _sut;

    public UnitServiceTest()
    {
        var repository = new EfUnintRepository(Context);
        var unitOfWork = new EfUnitOfWork(Context);
        var floorRepository = new EFFloorRepository(Context);
        _sut = new UnitServiceApp(floorRepository,repository, unitOfWork);
    }

    [Fact]
    public void Add_add_a_unit_properly()
    {
        var block=new BlockBuilder().WithFloorCount(1).Build();
        Save(block);
        var floor=new FloorBuilder().WithBlockID(block.Id).WithUnitCount(1).Build();
        Save(floor);
        var dto = new AddUnitDto()
        {
            Name = "asdf",
            FloorId = floor.Id
        };
        
        var actual= _sut.Add(dto);

        var resault = ReadContext.Set<Entities.Unit>().Single();

        actual.Should().Be(resault.Id);
    }

    [Fact]
    public void Add_Throw_exception_if_floor_does_not_exist()
    {
        
        var floor=new FloorBuilder().WithUnitCount(1).Build();
        
        var dto = new AddUnitDto()
        {
            Name = "asdf",
            FloorId = floor.Id
        };
        
        var actual=()=> _sut.Add(dto);
        
        actual.Should().ThrowExactly<FloorIsentExistException>();
    }
    
    [Fact]
    public void Add_Throw_exception_when_unit_is_exist_whith_this_name()
    {
        
        var block=new BlockBuilder().WithFloorCount(1).Build();
        Save(block);
        var floor=new FloorBuilder().WithBlockID(block.Id).WithUnitCount(2).Build();
        Save(floor);
        var unit=UnitFactory.Creat(floor.Id,"asdf");
        Save(unit);
        var dto = new AddUnitDto()
        {
            Name = "asdf",
            FloorId = floor.Id
        };
        
        var actual=()=> _sut.Add(dto);
        
        actual.Should().ThrowExactly<UnitIsExistWhitThisNameException>();
    }

    [Theory]
    [InlineData("asdf", "abc")]
    public void Update_update_a_unit_properly(string name,string updatedName)
    {
        var block=new BlockBuilder().WithFloorCount(1).Build();
        Save(block);
        var floor=new FloorBuilder().WithBlockID(block.Id).WithUnitCount(1).Build();
        Save(floor);
        var unit=UnitFactory.Creat(floor.Id,name);
        Save(unit);

        _sut.Update(name, updatedName);
        
        var resault = ReadContext.Set<Entities.Unit>().Single();
        
        resault.Name.Should().Be(updatedName);
    }
    
    [Theory]
    [InlineData("asdf", "abc")]
    public void Update_Throw_Exception_when_unit_isent_exist(string name,string updatedName)
    {
        var block=new BlockBuilder().WithFloorCount(1).Build();
        Save(block);
        var floor=new FloorBuilder().WithBlockID(block.Id).WithUnitCount(1).Build();
        Save(floor);
        var unit=UnitFactory.Creat(floor.Id,"name");
        Save(unit);

        var resault=()=> _sut.Update(name, updatedName);

        resault.Should().ThrowExactly<UnitIsNotExistWhitThisNameException>();
    }
    
    [Theory]
    [InlineData("asdf", "asdf")]
    public void Update_Throw_Exception_when_unit_is_repet_name(string name,string updatedName)
    {
        var block=new BlockBuilder().WithFloorCount(1).Build();
        Save(block);
        var floor=new FloorBuilder().WithBlockID(block.Id).WithUnitCount(1).Build();
        Save(floor);
        var unit=UnitFactory.Creat(floor.Id,name);
        Save(unit);

        var resault=()=> _sut.Update(name, updatedName);

        resault.Should().ThrowExactly<TheNameIsaLastNameException>();
    }

    [Fact]
    public void Delete_delete_a_unit_properly()
    {
        var block=new BlockBuilder().WithFloorCount(1).Build();
        Save(block);
        var floor=new FloorBuilder().WithBlockID(block.Id).WithUnitCount(1).Build();
        Save(floor);
        var unit=UnitFactory.Creat(floor.Id,"asdf");
        Save(unit);
        
        _sut.Delete(unit.Name);
        
        var resault = ReadContext.Set<Entities.Unit>().Any(_ => _.Name==unit.Name);
        
        resault.Should().BeFalse();
        
    }

    [Fact]
    public void Delete_Throw_exception_when_unit_is_not_exist_whith_name()
    {
        var block=new BlockBuilder().WithFloorCount(1).Build();
        Save(block);
        var floor=new FloorBuilder().WithBlockID(block.Id).WithUnitCount(1).Build();
        Save(floor);
        var unit=UnitFactory.Creat(floor.Id,"asdf");
        Save(unit);
        
        var actual=()=> _sut.Delete("Dumy");

        actual.Should().ThrowExactly<UnitIsNotExistWhitThisNameException>();
    }
    
    
    
    
    
    
    
    
    
    
}

public class AddUnitDto
{
    public string Name { get; set; }
    [Required]
    public int FloorId { get; set; }
}

public interface  UnitService
{
    int Add(AddUnitDto dto);
    void Update(string name, string updatedName);
    void Delete(string unitName);
}
public interface UnitRepository
{
    bool IsExsistByName(string dtoName);
    void Add(Entities.Unit unit);
    Entities.Unit FinfByName(string name);
    void Delete(string name);
}
public class UnitServiceApp(FloorRepository floorRepository,UnitRepository repository,UnitOfWork unitOfWork) : UnitService
{
    public int Add(AddUnitDto dto)
    {
        if (!floorRepository.IsExistById(dto.FloorId))
        {
            throw new
                FloorIsentExistException();
        }

        if (repository.IsExsistByName(dto.Name))
        {
            throw new
                UnitIsExistWhitThisNameException();
        }

        var unit = new Entities.Unit()
        {
            Name = dto.Name,
            FloorId = dto.FloorId,
        };
        repository.Add(unit);
        unitOfWork.Save();
        return unit.Id;
    }

    public void Update(string name, string updatedName)
    {
        if (!repository.IsExsistByName(name))
        {
            throw new
                UnitIsNotExistWhitThisNameException();
        }

        if (name==updatedName)
        {
            throw new
                TheNameIsaLastNameException();
        }
        var unit=repository.FinfByName(name);
        unit.Name = updatedName;
        unitOfWork.Save();
    }

    public void Delete(string unitName)
    {
        if (!repository.IsExsistByName(unitName))
        {
            throw new
                UnitIsNotExistWhitThisNameException();
        }

        repository.Delete(unitName);
        unitOfWork.Save();
    }
}

public class TheNameIsaLastNameException : Exception
{
}

public class UnitIsNotExistWhitThisNameException : Exception
{
}

public class UnitIsExistWhitThisNameException : Exception
{
}

public class EfUnintRepository(EfDataContext context):UnitRepository
{
    public bool IsExsistByName(string dtoName)
    {
        return context.Set<Entities.Unit>().Any(x => x.Name == dtoName);
    }

    public void Add(Entities.Unit dto)
    {
        context.Set<Entities.Unit>().Add(dto);
    }

    public Entities.Unit FinfByName(string name)
    {
        return context.Set<Entities.Unit>().Single(x => x.Name == name);
    }

    public void Delete(string name)
    {
        var unit = context.Set<Entities.Unit>().Single(x => x.Name == name);
        context.Set<Entities.Unit>().Remove(unit);
    }
}