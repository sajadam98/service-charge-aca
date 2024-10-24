using ServiceCharge.Persistence.Ef.Floors;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.Floors.Contracts.Dtos;

namespace ServiceCharge.Service.Unit.Tests.Floors;


public class FloorQueryTests:BusinessIntegrationTest
{
    private readonly FloorQuery _sut;

    public FloorQueryTests()
    {
        _sut = new EfFloorQuery(Context);
    }

    [Fact]
    public void GetAll_get_all_floors_properly()
    {
        var block = new Block()
        {
            Name = "CreateFloorWithUnits",
            FloorCount = 4,
            CreationDate = DateTime.UtcNow
        };
        Save(block);
        var floor = new Floor()
        {
            Name = "CreateFloorWithUnits",
            UnitCount = 4,
            BlockId = block.Id
        };
        Save(floor);
        var floor2 = new Floor()
        {
            Name = "name2",
            UnitCount = 8,
            BlockId = block.Id
        };
        Save(floor2);

        HashSet<GetFloorDto> result = _sut.GetAll();

        result.Should().HaveCount(2);
        result.Should()
            .Contain(_ => _.Id == floor.Id && _.Name == floor.Name);
        result.Should()
            .Contain(_ => _.Id == floor2.Id && _.Name == floor2.Name);
        result.First().Should().BeEquivalentTo(new GetFloorDto()
        {
            Id=floor.Id,
            Name = floor.Name,
            UnitCount = floor.UnitCount,
            BlockId = floor.BlockId
        });
    }

    [Fact]
    public void GetAllWithUnits_()
    {
        var block = new Block()
        {
            Name = "CreateFloorWithUnits",
            FloorCount = 8,
            CreationDate = DateTime.UtcNow
        };
        Save(block);
        var floor1 = new Floor()
        {
            BlockId = block.Id,
            Name = "dummy",
            UnitCount = 2
        };
        Save(floor1);
        var floor2 = new Floor()
        {
            BlockId = block.Id,
            Name = "dummy",
            UnitCount = 2
        };
        Save(floor2);
        var unit1 = new Entities.Unit()
        {
            FloorId = floor1.Id,
            Name = "golabi",
            IsActive = true
        };
        Save(unit1);
        var unit2 = new Entities.Unit()
        {
            FloorId = floor1.Id,
            Name = "golabi2",
            IsActive = true
        };
        Save(unit2);
        var unit3 = new Entities.Unit()
        {
            FloorId = floor1.Id,
            Name = "golabi3",
            IsActive = true
        };
        Save(unit3);
        var unit4 = new Entities.Unit()
        {
            FloorId = floor2.Id,
            Name = "golabi4",
            IsActive = true
        };
        Save(unit4);

        var resul = _sut.GetAllWithUnits();
        var actual =
            (from floor in ReadContext.Set<Floor>()
                join unit in ReadContext.Set<Entities.Unit>()
                    on floor.Id equals unit.FloorId
                select new
                {
                    Id = floor.Id,
                    Name = floor.Name,
                    UnitCount = floor.UnitCount,
                    BlockId = floor.BlockId,
                    Unit = unit
                })
            .GroupBy(_ => _.Id)
            .Select(_ => new
            {
                Id = _.Key,
                Name = _.First().Name,
                UnitCount = _.First().UnitCount,
                BlockId = _.First().BlockId,
                Units = _.Select(_=>_.Unit).ToHashSet()
            })
            .ToHashSet();

        resul.Should().HaveCount(actual.Count);
        resul.First().Units.Should().HaveCount(actual.First().Units.Count);
        resul.Should().Contain(_ => _.Id == floor1.Id && _.Name == floor1.Name);
        resul.Should().Contain(_ => _.Id == floor2.Id && _.Name == floor2.Name);
    }
}