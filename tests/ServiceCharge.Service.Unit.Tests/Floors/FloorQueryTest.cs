namespace ServiceCharge.Service.Unit.Tests.Floors;

public class FloorQueryTest : BusinessIntegrationTest
{
    private readonly FloorQuery _sut;

    public FloorQueryTest()
    {
        _sut = FloorQueryFactory.CreateQuery(SetupContext);
    }

    [Fact]
    public void GetAll_get_all_floors_properly()
    {
        var block = BlockFactory.Create();
        Save(block);
        var block2 = BlockFactory.Create();
        Save(block2);
        var floor = FloorFactory.Create(blockId: block.Id);
        Save(floor);
        var floor2 = FloorFactory.Create(blockId: block2.Id);
        Save(floor2);

        var actual = _sut.GetAll();

        var excepted = ReadContext.Set<Floor>()
            .Select(_ => new GetFloorDto()
            {
                Id = _.Id,
                Name = _.Name,
                UnitCount = _.UnitCount,
                BlockId = _.BlockId
            });
        excepted.Should().HaveCount(2);
        // excepted.Should()
        //     .Contain(_ => _.Id == floor.Id && _.BlockId == block.Id);
        // excepted.Should()
        //     .Contain(_ => _.Id == floor2.Id && _.BlockId == block2.Id);
        excepted.Should().BeEquivalentTo(actual);
    }
    
}