using FluentAssertions;
using ServiceCharge.Entities;
using ServiceCharge.Persistence.Ef;
using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;

namespace ServiceCharge.Service.Unit.Tests.Floors;

public class FloorQueryTest:BusinessIntegrationTest
{
    private readonly FloorQuery _sut;

    public FloorQueryTest()
    {
        _sut=new EfFloorQuery(Context);
    }
    
    
    [Theory]
    [InlineData("Yas")]
    [InlineData("Yass")]
    public void Read_read_a_floor_properly(string floorName)
    {
        var block=new BlockBuilder().WithFloorCount(2).Build();
        Save(block);
        var floor = FloorFactory.Create(block.Id, "Yas");
        Save(floor);
        var floor2 = FloorFactory.Create(block.Id, "Yass");
        Save(floor2);
        var actual = _sut.GetAll();

        var expected = ReadContext.Set<Floor>().Select(_=>FloorFactory.GetAll(_.BlockId,_.Name,_.UnitCount)).ToHashSet();

        actual.Should().HaveCount(2);
        actual.Should().BeEquivalentTo(expected);

    }
}



public interface FloorQuery
{
    HashSet<GetAllFloorDto> GetAll();
}


public class EfFloorQuery(EfDataContext context) : FloorQuery
{
    public HashSet<GetAllFloorDto> GetAll()
    {
        return context.Set<Floor>().Select(_=>new GetAllFloorDto
        {
            Name = _.Name,
            BlockId = _.BlockId,
            UnitCout = _.UnitCount
        }).ToHashSet();
    }
}