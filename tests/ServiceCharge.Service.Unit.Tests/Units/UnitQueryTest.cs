using FluentAssertions;
using ServiceCharge.Persistence.Ef;
using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;
using ServiceCharge.TestTools.Units;

namespace ServiceCharge.Service.Unit.Tests.Units;

public class UnitQueryTest:BusinessIntegrationTest
{
    private readonly UnitQuery _sut;

    public UnitQueryTest()
    {
        _sut = new EfUnitQuery(Context);
    }

    [Theory]
    [InlineData("asf","as")]
    public void Get_Get_all_Units(string name,string name2)
    {
        var block=new BlockBuilder().WithFloorCount(1).Build();
        Save(block);
        var floor=new FloorBuilder().WithBlockID(block.Id).WithUnitCount(2).Build();
        Save(floor);
        var unit=UnitFactory.Creat(floor.Id,name);
        Save(unit);
        var unit2=UnitFactory.Creat(floor.Id,name2);
        Save(unit2);

        var actual = _sut.ReadAll();
        var except = ReadContext.Set<Entities.Unit>().Select(_ => new GetAllUnit()
        {
            Name = _.Name,
            FloorId = _.FloorId,
            IsActive = _.IsActive,
        }).ToHashSet();

        actual.Should().HaveCount(2);
        actual.Should().BeEquivalentTo(except);

    }
    
    
}

public interface UnitQuery 
{
    HashSet<GetAllUnit> ReadAll();
}

public class GetAllUnit
{
    public int FloorId { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
}

public class EfUnitQuery(EfDataContext context):UnitQuery
{
    public HashSet<GetAllUnit> ReadAll()
    {
        return context.Set<Entities.Unit>().Select(_ => new GetAllUnit()
        {
            Name = _.Name,
            FloorId = _.FloorId,
            IsActive = _.IsActive
        }).ToHashSet();
    }
}