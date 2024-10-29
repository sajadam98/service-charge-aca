using FluentAssertions;
using ServiceCharge.Persistence.Ef.Floors;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;
using ServiceCharge.TestTools.Floors.DTOs;

namespace ServiceCharge.Service.Unit.Tests.Floors;

public class FloorQueryTests : BusinessIntegrationTest
{
    private readonly FloorQuery _sut;

    public FloorQueryTests()
    {
        _sut = FloorQueryFactory.CreateQuery(SetupContext);
    }

    [Fact]
    public void GetAll_gets_all_floors_properly()
    {
        var block = new BlockBuilder().Build();
        Save(block);
        var floor1 = new FloorBuilder(block.Id).Build();
        Save(floor1);
        var floor2 = new FloorBuilder(block.Id).Build();
        Save(floor2);

        var actual = _sut.GetAll();

        actual.Should().HaveCount(2);
        actual.Should().ContainEquivalentOf(
            new GetAllFloorsDtoBuilder(floor1.Id, floor1.BlockId).Build());
        actual.Should().ContainEquivalentOf(
            new GetAllFloorsDtoBuilder(floor2.Id, floor2.BlockId).Build());
    }
}